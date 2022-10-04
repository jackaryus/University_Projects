// File Author: Daniel Masterson
using SpaceGame.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    /// <summary>
    /// Details about an AI's opinion towards another player
    /// </summary>
    public class PlayerOpinion
    {
        public float Opinion = 0.0f;
        public float Suspicion = 0.0f;

        public PlayerOpinion(float initialOpinion)
        {
            Opinion = initialOpinion;
        }
    }

    /// <summary>
    /// A player that is automatically controlled
    /// </summary>
    public class AIPlayer : Player
    {
        // The time before the AI will attempt to perform any actions. If this is <= 0, the AI will act.
        // Decays with the frame delta
        float aiActionTimeout = 0.0f;

        const float timeCost_Random = 1.2f;
        const float timeCost_Capture = 6.0f;

        float opinion_initalMin = 0.0f;
        float opinion_initialMax = 45.0f;
        float opinion_regenRate = 0.5f;
        //If a player goes below this, opinion no longer recovers
        float opinion_enemyThresh = -25.0f;

        //The opinion pentalty another player will incur by captuing one of my planets
        //Penalty occurs each AI action
        float opinion_capturePenalty = -2.5f;
        //Penalty incurred when another player destroys one of my ships
        float opinion_shipDestroyPenalty = -2.5f;

        //A penalty applied for each idle ship surrounding one of my planets that hasn't recently emoted happy
        float opinion_idleShipPenalty = -0.25f;

        //How fast the suspicion value decays
        float suspicion_decayRate = 0.01f;
        //How fast suspicion increases per ship in an idle orbit around one of my planets
        float suspicion_idleShipIncreaseRate = 0.02f;

        //The distance where automatic defending is guaranteed
        float defend_definiteRadius = 256.0f;

        //Negatives = Hostile, Positives = Friendly
        Dictionary<Player, PlayerOpinion> playerOpinion = new Dictionary<Player, PlayerOpinion>();

        public AIPlayer(TeamInfo ti)
            : base(ti)
        {
        }

        protected override void OnSpawn()
        {
            opinion_initalMin = SettingsManager.GetSetting<float>("ai.opinion.minInitialValue");
            opinion_initialMax = SettingsManager.GetSetting<float>("ai.opinion.maxInitialValue");
            opinion_regenRate = SettingsManager.GetSetting<float>("ai.opinion.regen");
            opinion_enemyThresh = SettingsManager.GetSetting<float>("ai.opinion.enemyThreshold");
            opinion_capturePenalty = SettingsManager.GetSetting<float>("ai.opinion.capturePenalty");
            opinion_shipDestroyPenalty = SettingsManager.GetSetting<float>("ai.opinion.shipDestroyPenalty");
            opinion_idleShipPenalty = SettingsManager.GetSetting<float>("ai.opinion.idleShipPenalty");

            suspicion_decayRate = SettingsManager.GetSetting<float>("ai.suspicion.decayRate");
            suspicion_idleShipIncreaseRate = SettingsManager.GetSetting<float>("ai.suspicion.idleShipIncreaseRate");

            defend_definiteRadius = SettingsManager.GetSetting<float>("ai.defence.definiteEngageRadius");

            for (int i = 0; i < PlayerManager.Players.Count; ++i)
            {
                if (PlayerManager.Players[i] != this)
                {
                    float initialOpinion = opinion_initalMin + (float)UtilityManager.Random.NextDouble() * (opinion_initialMax - opinion_initalMin);
                    playerOpinion.Add(PlayerManager.Players[i], new PlayerOpinion(initialOpinion));
                }
            }
        }

        protected override void OnUpdate(float delta)
        {
            //Don't do anything if we've reached an endgame state
            if (PlayerState == EPlayerState.Won || PlayerState == EPlayerState.Lost)
                return;

            UpdatePlayerOpinion(delta);

            aiActionTimeout -= delta;

            if (aiActionTimeout <= 0)
                DoAI(delta);
        }

        protected void DoAI(float delta)
        {
            /********************************************************************************
             * This is the majority of AI logic and works as follows
             * 
             * For each node we own:
             *  - If someone is capturing it
             *  -- Apply a negative opinion to this player
             *  -- Set maximum suspcion for this player
             *  -- Potentially send some of our ships to defend the planet, depending on
             *     the ship's proximity to the captured plant. Attack if we're orbiting this
             *     planet
             *  -- Update the aiActionTimeout, but set it to half of what it normally would be
             *  -- Return
             *  - Otherwise, if there are idle ships around it (Other player ships that are not
             *    capturing
             *  -- Adjust our opinion and suspicion for each orbiting ship's player, based on that
             *     ship's "happiness modifier"
             *        This happiness modifier is a value that each ship stores ranging from -1.0
             *        to +1.0, with -1.0 being if that ship has just emoted angrilly, and +1.0
             *        if they have just emoted happilly, which then gradually decays back to 0
             *        (Unhappy emote sets it to 0 straight away).
             *        
             *        Ergo, if a ship has just emoted happilly, that can be a sign of friendliness
             *        and so the corrected happiness modifier is 0 (Suspicion and Opinion don't change).
             *        On the other hand, if the ship has just emoted angrilly, this can be a sign of
             *        aggression, and so the correct happiness modifier is 2 (Suspicion and Opinion
             *        change at twice the normal rate)
             *        
             *  - After this, the AI then has a very low chance to move a random ship to a random
             *     nearby unoccupied or planet not occupied by itself.
             *  -- If the planet is occupied by another player, the chance is modified based on
             *     the AI's opinion of the player. The lower the opinion the AI has against this
             *     player, the more likely they are to capture the planet.
             *  -- If the AI then successfully moves a ship, the aiActionTimeout is set to its
             *     normal value
             *  -- If the AI doesn't move a ship, then they will try again next frame
             *     
             ********************************************************************************/

            for (int i = 0; i < OwnedNodes.Count; ++i)
            {
                if (OwnedNodes[i].CapturingPlayer != null) //Someone's capturing my planet!
                {
                    playerOpinion[OwnedNodes[i].CapturingPlayer].Opinion = Math.Max(-100.0f, playerOpinion[OwnedNodes[i].CapturingPlayer].Opinion + opinion_capturePenalty);
                    playerOpinion[OwnedNodes[i].CapturingPlayer].Suspicion = 1.0f;

                    for (int s = 0; s < ownedShips.Count; ++s)
                    {
                        if (ownedShips[s].targetNode == OwnedNodes[i])
                        {
                            if (ownedShips[s].shipState == Ship.EShipState.Orbiting)
                            {
                                ownedShips[s].BeginAttack();
                                ownedShips[s].Emote("angry");
                            }
                        }
                        else
                        {
                            float dist = (ownedShips[s].Center - OwnedNodes[i].Center).Length();

                            if (defend_definiteRadius < UtilityManager.Random.NextDouble() * dist)
                            {
                                ownedShips[s].SetTargetNode(OwnedNodes[i]);
                                ownedShips[s].Emote("exclamation");
                            }
                        }
                    }

                    aiActionTimeout = (float)(timeCost_Capture + UtilityManager.Random.NextDouble() * timeCost_Random) * 0.5f;
                    return;
                }
                else if (OwnedNodes[i].IdleShips.Count > 0)
                {
                    for (int s = 0; s < OwnedNodes[i].IdleShips.Count; ++s)
                    {
                        float happinessModifier = 1.0f - OwnedNodes[i].IdleShips[s].HappinessRating;

                        playerOpinion[OwnedNodes[i].IdleShips[s].OwningPlayer].Suspicion += suspicion_idleShipIncreaseRate * happinessModifier * delta;
                        playerOpinion[OwnedNodes[i].IdleShips[s].OwningPlayer].Opinion += opinion_idleShipPenalty * happinessModifier * delta;
                    }
                }

                //Potentially perform a ship movement action
                if (UtilityManager.Random.NextDouble() < 0.1)
                {
                    for (int j = 0; j < OwnedNodes[i].nodeConnections.Count; ++j)
                    {
                        Node target = OwnedNodes[i].nodeConnections[j].other;
                        if (WantToCaptureNode(target))
                        {
                            Ship nearestShip = ownedShips[0];
                            for (int s = 1; s < ownedShips.Count; ++s) //Find the nearest ship
                            {
                                if ((ownedShips[s].Center - target.Center).Length() < (nearestShip.Center - target.Center).Length())
                                    nearestShip = ownedShips[s];
                            }

                            nearestShip.SetTargetNode(OwnedNodes[i].nodeConnections[j].other);
                            aiActionTimeout = (float)(timeCost_Capture + UtilityManager.Random.NextDouble() * timeCost_Random);

                            return;
                        }
                    }
                }
            }
        }

        /// <summary>Checks whether we should capture this node</summary>
        /// <param name="node">The node to potentially capture</param>
        /// <returns>True if we're capturing, false if not</returns>
        protected bool WantToCaptureNode(Node node)
        {
            if (node.CurrentOwner == this)
                return false;

            // Node is unoccupied
            if (node.CurrentOwner == null)
            {
                if (UtilityManager.Random.NextDouble() < 0.1)
                    return true;

                return false;
            }

            // Node is occupied by another player. Calculate whether we want to try to capure their node
            float enemyDepth = ((playerOpinion[node.CurrentOwner].Opinion - 100) * -1) / 200; //Gets a scale from 0-1, where 0 is perfect friend, and 1 is perfect enemy
            enemyDepth *= enemyDepth; //Power of 2

            if (UtilityManager.Random.NextDouble() < enemyDepth)
                return true;

            return false;
        }

        /// <summary>Updates opinion and suspicion for each player each frame</summary>
        /// <param name="delta">Frame delta</param>
        protected void UpdatePlayerOpinion(float delta)
        {
            for (int i = 0; i < PlayerManager.Players.Count; ++i)
            {
                if (PlayerManager.Players[i] == this)
                    continue;

                playerOpinion[PlayerManager.Players[i]].Suspicion = Math.Max(0.0f, playerOpinion[PlayerManager.Players[i]].Suspicion - suspicion_decayRate * delta);

                float opinionChange = playerOpinion[PlayerManager.Players[i]].Opinion + delta * (opinion_regenRate * (1.0f - playerOpinion[PlayerManager.Players[i]].Suspicion) * 2.0f - 1.0f);
                if (opinionChange > 0 && playerOpinion[PlayerManager.Players[i]].Opinion > opinion_enemyThresh)
                    playerOpinion[PlayerManager.Players[i]].Opinion = Math.Min(100.0f, Math.Max(-100.0f, opinionChange));
            }
        }

        /// <summary>
        /// Allows the AI to act when their ship is destroyed
        /// </summary>
        /// <param name="causer">The ship (if any) that destroyed my ship</param>
        public override void NotifyShipDestroyed(Ship causer)
        {
            playerOpinion[causer.OwningPlayer].Opinion += opinion_shipDestroyPenalty;
            playerOpinion[causer.OwningPlayer].Suspicion = 1.0f;
        }

        public override void NotifyResolutionChanged()
        {
            //Blanked because AI doesn't have a UI
        }

        /// <summary>
        /// Returns this player's affiliation with another player
        /// </summary>
        /// <param name="other">The player we're getting an opinion for</param>
        /// <returns>This players opinion</returns>
        public override string GetOpinion(Player other)
        {
            if (playerOpinion[other].Opinion > 50)
                return "Friend";
            else if (playerOpinion[other].Opinion > 25)
                return "Acquaintance";
            else if (playerOpinion[other].Opinion > -10)
                return "Neutral";
            else if (playerOpinion[other].Opinion > opinion_enemyThresh)
                return "Wary";
            else
                return "Enemy";
        }
    }
}
