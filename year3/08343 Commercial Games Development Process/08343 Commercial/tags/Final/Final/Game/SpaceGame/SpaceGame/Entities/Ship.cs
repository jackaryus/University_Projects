// File Author: Daniel Masterson
using Microsoft.Xna.Framework;
using SpaceGame.Components;
using SpaceGame.GameManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    public class Ship : Entity
    {
        public enum EShipState
        {
            Dead,
            Orbiting,
            Travelling, 
            Attacking,
        }

        public string Name { get; private set; }

        public Node targetNode { get; private set; }

        ButtonComponent buttonComp;
        LabelComponent name_LabelComp;
        ImageComponent imageComp, imageColComp, emoteComp;


        public Vector2 velocity { get; private set; }
        public const float MaxVelocity = 24.0f;
        public const float OrbitVelocity = 1.2f;

        public EShipState shipState { get; private set; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public float Fuel { get; private set; }
        public float MaxFuel { get; private set; }

        public float FuelDrainPerUnitTraveled { get; private set; }
        public float FuelRestoreRate { get; private set; }

        public float CaptureModifier { get; private set; }

        public float AttackTimeout { get; private set; }
        public float MinAttackDamage { get; private set; }
        public float MaxAttackDamage { get; private set; }
        float CurrentAttackTimeout = 0.0f;

        //1 = Just emoted happy, -1 = Just emoted angry
        public float HappinessRating { get; private set; }
        const float happinessRatingDecay = 0.01f;
        float timeSinceLastEmote = 0.0f;
        const float minTimeToIdleEmote = 24.0f;

        protected override void OnSpawn()
        {
            SetScale(new Vector2(64, 64));

            RegisterComponent(imageComp = new ImageComponent("ship_1_u"))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetZIndex(11);

            RegisterComponent(imageColComp = new ImageComponent("ship_1_o"))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetZIndex(12);

            RegisterComponent(emoteComp = new ImageComponent("emote_ellipses"))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetZIndex(13)
                .SetScale(new Vector2(64.0f, 64.0f))
                .SetVisible(false);

            RegisterComponent(buttonComp = new ButtonComponent("", "generic", Color.White, 1.0f, Color.Gray, Color.AliceBlue, Color.Blue, () =>
            {
                if (OwningPlayer == PlayerManager.LocalPlayer)
                    OwningPlayer.SetSelectedObject(this);
            }))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetTexture("glow")
                .SetZIndex(10);

            RegisterComponent(name_LabelComp = new LabelComponent("UNDEFINED", "generic", Color.White, 1.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Top))
                .SetRenderSpace(ERenderSpace.WorldSpace)
                .SetZIndex(11);


            Health = MaxHealth = SettingsManager.GetSetting<float>("ship.health");
            Fuel = MaxFuel = SettingsManager.GetSetting<float>("ship.fuel");
            FuelDrainPerUnitTraveled = SettingsManager.GetSetting<float>("ship.fuelDrain");
            FuelRestoreRate = SettingsManager.GetSetting<float>("ship.fuelRestore");

            CaptureModifier = SettingsManager.GetSetting<float>("ship.captureModifier");

            AttackTimeout = SettingsManager.GetSetting<float>("ship.attackTimeout");
            MinAttackDamage = SettingsManager.GetSetting<float>("ship.minAttackDamage");
            MaxAttackDamage = SettingsManager.GetSetting<float>("ship.maxAttackDamage");
        }

        protected override void OnUpdate(float delta)
        {
            if (shipState == EShipState.Dead)
                return;

            if (targetNode != null)
            {
                float newLength = velocity.Length();
                Vector2 targetPosition = targetNode.Center;

                if (shipState == EShipState.Travelling && (targetNode.Center - Center).Length() < 128)
                {
                    shipState = EShipState.Orbiting;
                    targetNode.BeginMining(this);
                }

                if (shipState == EShipState.Orbiting || shipState == EShipState.Attacking)
                {
                    newLength = UtilityManager.Lerp(newLength, OrbitVelocity, 0.02f);
                    float currentPlanetAngle = (float)Math.Atan2(Center.Y - targetPosition.Y, Center.X - targetPosition.X) + OrbitVelocity;
                    targetPosition = new Vector2(
                        targetPosition.X + (float)Math.Cos(currentPlanetAngle) * 128,
                        targetPosition.Y + (float)Math.Sin(currentPlanetAngle) * 128
                        );
                }
                else
                {
                    newLength = UtilityManager.Lerp(newLength, MaxVelocity, 0.02f);
                    Fuel = Math.Max(0.0f, Fuel - newLength * FuelDrainPerUnitTraveled);
                }

                Vector2 newVelocity = Vector2.Normalize(targetPosition - Center) * newLength;

                if (float.IsNaN(newVelocity.X) || float.IsNaN(newVelocity.Y))
                    newVelocity = Vector2.Zero;

                velocity = Vector2.Lerp(velocity, newVelocity, 0.2f);

                SetPosition(Position + velocity);

                CurrentAttackTimeout = Math.Max(0.0f, CurrentAttackTimeout - delta);

                if (shipState == EShipState.Attacking)
                {
                    if (CurrentAttackTimeout <= 0.0f)
                    {
                        List<Ship> allShips = new List<Ship>();
                        allShips.AddRange(targetNode.MiningShips.Where((s) => s.shipState != EShipState.Dead && s.OwningPlayer != OwningPlayer));
                        allShips.AddRange(targetNode.IdleShips.Where((s) => s.shipState != EShipState.Dead && s.OwningPlayer != OwningPlayer));
                        allShips.AddRange(targetNode.CapturingShips.Where((s) => s.shipState != EShipState.Dead && s.OwningPlayer != OwningPlayer));

                        if (allShips.Count > 0)
                        {
                            DataManager.Output(OwningPlayer.Team.Name + "'s ship '" + Name + "'(" + Health + "/" + MaxHealth + ") attacks surrounding ships.");

                            for (int i = 0; i < allShips.Count; ++i)
                            {
                                allShips[i].OnAttack(this);
                            }

                            CurrentAttackTimeout = AttackTimeout;
                        }
                        else
                        {
                            DataManager.Output(OwningPlayer.Team.Name + "'s ship '" + Name + "' stops attacking (No surrounding ships).");
                            shipState = EShipState.Orbiting;
                        }
                    }
                }
            }

            if (Fuel < MaxFuel && OwningPlayer.HasResource(EResourceType.Fuel))
            {
                Fuel -= OwningPlayer.AddResources(EResourceType.Fuel, -FuelRestoreRate * delta);
            }

            //Idle emote
            timeSinceLastEmote += delta;
            if (OwningPlayer is AIPlayer && timeSinceLastEmote > minTimeToIdleEmote && UtilityManager.Random.NextDouble() < 0.005)
            {
                Emote("*");
            }

            HappinessRating = UtilityManager.Lerp(HappinessRating, 0.0f, happinessRatingDecay);

            imageComp.SetPositionScale(Position + Scale * 0.1f, Scale * 0.8f);
            imageColComp.SetPositionScale(Position + Scale * 0.1f, Scale * 0.8f);
            emoteComp.SetPosition(new Vector2(imageComp.Position.X + imageComp.Scale.X, imageComp.Position.Y - emoteComp.Scale.Y)).SetRotation(-Rotation);
            buttonComp.SetPositionScale(Position, Scale);
            name_LabelComp.SetPosition(new Vector2(Center.X, Position.Y + Scale.Y + 6));
            SetRotation((float)(Math.Atan2(velocity.Y, velocity.X) + Math.PI / 2));
        }

        public Ship SetTargetNode(Node newNode, bool instantTravel = false)
        {
            //First check if the ship has enough fuel to get there
            float fuelCost = instantTravel ? 0 : (Center - newNode.Center).Length() * FuelDrainPerUnitTraveled;

            if (fuelCost > Fuel)
            {
                OwningPlayer.Notification("Ship " + Name + " doesn't have enough fuel to get to " + targetNode.Name, Color.Red);
                return this;
            }

            if (!instantTravel)
            {
                if (OwningPlayer == PlayerManager.LocalPlayer)
                    DataManager.Output(PlayerManager.LocalPlayer.Team.Name + "(Player) moved " + this.Name + " to planet " + newNode.Name);
                else
                    DataManager.Output(OwningPlayer.Team.Name + "(AI) moved " + this.Name + " to planet " + newNode.Name);
            }

            if (targetNode != null && (shipState == EShipState.Orbiting || shipState == EShipState.Attacking))
            {
                targetNode.StopMining(this);
            }

            targetNode = newNode;

            if (instantTravel)
            {
                Vector2 randOffset = new Vector2((float)(UtilityManager.Random.NextDouble() * 2 - 1), (float)(UtilityManager.Random.NextDouble() * 2 - 1));
                randOffset.Normalize();
                randOffset *= newNode.Scale.X;
                SetPosition(newNode.Center + randOffset, true);
            }
            
            shipState = EShipState.Travelling;

            return this;
        }

        public Ship SetName(string newName)
        {
            Name = newName;
            name_LabelComp.SetText(newName);

            return this;
        }

        public string GetRandomName(Player plr)
        {
            string finalName = "";

            for (int i = 0; i < plr.Team.Name.Length; i++)
            {
                if (char.IsUpper(plr.Team.Name[i]))
                    finalName += plr.Team.Name[i];
            }

            finalName += " " + UtilityManager.Random.Next(100, 1000);

            return finalName;
        }

        public override bool OnSetOwningPlayer(Player newOwner)
        {
            if (OwningPlayer != null)
                OwningPlayer.RemoveOwnedShip(this);
            newOwner.AddOwnedShip(this);

            imageColComp.SetTint(newOwner.Team.Color);
            buttonComp.IdleColor = newOwner.Team.Color * 0.7f;
            buttonComp.HoverColor = newOwner.Team.Color * 1.0f;
            buttonComp.DownColor = newOwner.Team.Color * 0.5f;

            imageComp.SetTexture("ship_" + newOwner.Team.Ship + "_u");
            imageColComp.SetTexture("ship_" + newOwner.Team.Ship + "_o");

            SetName(GetRandomName(newOwner));

            return true;
        }

        public void BeginAttack()
        {
            if (shipState == EShipState.Orbiting)
                shipState = EShipState.Attacking;
        }

        public void OnAttack(Ship attacker)
        {
            float lastHealth = Health;
            Health -= (float)Math.Max(0.0f, attacker.MinAttackDamage + UtilityManager.Random.NextDouble() * (attacker.MaxAttackDamage - attacker.MinAttackDamage));

            if (Health < 1.0) //We do int rounding everywhere, so 0.9 would display as 0 but still be a functioning ship
            {
                shipState = EShipState.Dead;
                attacker.OwningPlayer.Notification("Destroyed " + OwningPlayer.Team.Name + "'s ship " + Name + "!", Color.Green);
                OwningPlayer.Notification(attacker.OwningPlayer.Team.Name + " destroyed your ship " + Name + "!", Color.Red);
                OwningPlayer.NotifyShipDestroyed(attacker);
                Destroy(this);
                return;
            }
            else if(lastHealth > MaxHealth * 0.2f && Health < MaxHealth * 0.2f)
            {
                OwningPlayer.Notification("Ship " + Name + " is low on health!", Color.Red);
            }

            if (shipState == EShipState.Orbiting)
            {
                DataManager.Output(OwningPlayer.Team.Name + "'s ship '" + Name + "'(" + (int)Health + "/" + (int)MaxHealth + ") retaliates against an attack by " + attacker.OwningPlayer.Team.Name + "'s ship '" + Name + "'(" + (int)Health + "/" + (int)MaxHealth + ")");
                shipState = EShipState.Attacking;
                CurrentAttackTimeout = AttackTimeout;
            }
        }

        public void Emote(string emote)
        {
            if (emote == "*") //Special context emote
            {
                if (Health < MaxHealth * 0.2f)
                    emote = "unhappy";
                else if (shipState == EShipState.Attacking)
                    emote = "angry";
                else if (UtilityManager.Random.NextDouble() < 0.25f)
                    emote = "happy";
                else
                    emote = "ellipses";
            }

            if (emote == "happy")
                HappinessRating = 1.0f;
            else if (emote == "angry")
                HappinessRating = -1.0f;
            else if (emote == "unhappy")
                HappinessRating = 0.0f;

            emoteComp.SetTexture("emote_" + emote).SetVisible(true).FadeOut(1.0f, false, 2.0f);
            DataManager.Output(OwningPlayer.Team.Name + "(" + (OwningPlayer == PlayerManager.LocalPlayer ? "Player" : "AI") + ") emoted '" + emote + "'");
            timeSinceLastEmote = 0;
        }

        protected override void OnDestroy()
        {
            DataManager.Output(OwningPlayer.Team.Name + "'s ship " + this.Name + " was destroyed");
            OwningPlayer.RemoveOwnedShip(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
