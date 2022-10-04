// File Author: Daniel Masterson
using SpaceGame.Components;
using SpaceGame.GameManagers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Entities
{
    /// <summary>
    /// The main menu
    /// </summary>
    public class MainMenu : Entity
    {
        LabelComponent title_LabelComp;

        ButtonComponent twoPlayer_ButtonComp, fourPlayer_ButtonComp, eightPlayer_ButtonComp, exit_ButtonComp;
        LabelComponent twoPlayer_LabelComp, fourPlayer_LabelComp, eightPlayer_LabelComp, exit_LabelComp;

        protected override void OnSpawn()
        {
            title_LabelComp = new LabelComponent(Main.GameTitle, "generic", Color.Aqua, 8.0f, ETextHorizontalAlign.Center, ETextVerticalAlign.Bottom);
            title_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(title_LabelComp);

            twoPlayer_ButtonComp = new ButtonComponent("1v1 Match", "generic", Color.Aqua, 2.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
            {
                SceneManager.LoadScene(new CustomiseMenuScene(EGameType.OneVsOne));
            }, () => { twoPlayer_LabelComp.SetVisible(true); }, () => { twoPlayer_LabelComp.SetVisible(false); });
            twoPlayer_ButtonComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(twoPlayer_ButtonComp);

            twoPlayer_LabelComp = new LabelComponent("A one versus one match with a random civilization", "generic", Color.Aqua, 1.5f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            twoPlayer_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace).SetVisible(false);
            RegisterComponent(twoPlayer_LabelComp);

            fourPlayer_ButtonComp = new ButtonComponent("4 Player Match", "generic", Color.Aqua, 2.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
            {
                SceneManager.LoadScene(new CustomiseMenuScene(EGameType.FourPlayer));
            }, () => { fourPlayer_LabelComp.SetVisible(true); }, () => { fourPlayer_LabelComp.SetVisible(false); }); 
            fourPlayer_ButtonComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(fourPlayer_ButtonComp);

            fourPlayer_LabelComp = new LabelComponent("You and three other civilizations fight it out", "generic", Color.Aqua, 1.5f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            fourPlayer_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace).SetVisible(false);
            RegisterComponent(fourPlayer_LabelComp);

            eightPlayer_ButtonComp = new ButtonComponent("8 Player Match", "generic", Color.Aqua, 2.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
            {
                SceneManager.LoadScene(new CustomiseMenuScene(EGameType.EightPlayer));
            }, () => { eightPlayer_LabelComp.SetVisible(true); }, () => { eightPlayer_LabelComp.SetVisible(false); });
            eightPlayer_ButtonComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(eightPlayer_ButtonComp);

            eightPlayer_LabelComp = new LabelComponent("You and seven other civilizations fight it out", "generic", Color.Aqua, 1.5f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            eightPlayer_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace).SetVisible(false);
            RegisterComponent(eightPlayer_LabelComp);

            exit_ButtonComp = new ButtonComponent("Exit", "generic", Color.Aqua, 2.0f, Color.Blue * 0.5f, Color.Cyan * 0.5f, Color.Blue * 0.8f, () =>
            {
                Main.self.Exit();
            }, () => { exit_LabelComp.SetVisible(true); }, () => { exit_LabelComp.SetVisible(false); });
            exit_ButtonComp.SetRenderSpace(ERenderSpace.ScreenSpace);
            RegisterComponent(exit_ButtonComp);

            exit_LabelComp = new LabelComponent("Close the game", "generic", Color.Aqua, 1.5f, ETextHorizontalAlign.Center, ETextVerticalAlign.Middle);
            exit_LabelComp.SetRenderSpace(ERenderSpace.ScreenSpace).SetVisible(false);
            RegisterComponent(exit_LabelComp);
        }

        public override void NotifyResolutionChanged()
        {
            title_LabelComp.SetPosition(new Vector2(Main.Resolution.X / 2, Main.Resolution.Y * 0.35f));
            twoPlayer_ButtonComp.SetPositionScale(new Vector2(60, Main.Resolution.Y * 0.4f), new Vector2(256.0f, 64.0f));
            twoPlayer_LabelComp.SetPosition(new Vector2(720.0f, Main.Resolution.Y * 0.45f));
            fourPlayer_ButtonComp.SetPositionScale(new Vector2(65, Main.Resolution.Y * 0.5f), new Vector2(251.0f, 64.0f));
            fourPlayer_LabelComp.SetPosition(new Vector2(720.0f, Main.Resolution.Y * 0.55f));
            eightPlayer_ButtonComp.SetPositionScale(new Vector2(81, Main.Resolution.Y * 0.6f), new Vector2(235.0f, 64.0f));
            eightPlayer_LabelComp.SetPosition(new Vector2(720.0f, Main.Resolution.Y * 0.65f));
            exit_ButtonComp.SetPositionScale(new Vector2(105, Main.Resolution.Y * 0.8f), new Vector2(211.0f, 64.0f));
            exit_LabelComp.SetPosition(new Vector2(780.0f, Main.Resolution.Y * 0.85f));
        }
    }
}
