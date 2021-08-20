
using UnityEditor;
using UnityEngine;

namespace OpenMMO
{
    //LAUNCH POPUP
    class HelloOpenMMO : AssetPostprocessor
    {
        static bool hasOpened = false;
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (hasOpened) return;

            HelloOpenMMOPopup popup = (HelloOpenMMOPopup)EditorWindow.GetWindow(typeof(HelloOpenMMOPopup), true, "OPENMMO - The Free Open Source Networked Game Framework");
            popup.maxSize = new Vector2(488f, 512f);
            popup.minSize = popup.maxSize;
            hasOpened = true;
        }
    }

    //POPUP TEXT
    public class HelloOpenMMOPopup : EditorWindow
    {
        private void OnGUI()
        {
            //WELCOME MESSAGE
            EditorGUILayout.TextField("WELCOME TO OPENMMO!", EditorStyles.largeLabel);
            EditorGUILayout.LabelField(
                "Thank you for choosing OpenMMO for your game project!"
                + "\n"
                + "\n" + "By now, thousands of hours have gone into the development of OpenMMO..."
                + "\n" + "please help support this project in any way you are able!"
                + "\n"
                + "\n" + "Even just a small monthly subscription on Patreon makes a huge difference "
                + "\n" + "to the project. We're not even asking for beer money here - we've all got "
                + "\n" + "day jobs - we need funding for the OpenMMO project itself. "
                + "\n"
                + "\n" + "Our team wants to see OpenMMO continue to grow. A big part of that is basic "
                + "\n" + "advertising, which unfortunately comes with a price-tag of both time and money. "
                + "\n" + "Since our efforts are best spent coding and adding to the OpenMMO framework, "
                + "\n" + "your donations will help us set up a number of campaigns to promote OpenMMO "
                + "\n" + "and make it better. Just check out our Patreon milestones to see what our goals "
                + "\n" + "are."
                + "\n"
                + "\n" + "Aside from money, there are also other ways to contribute. We ALWAYS need more "
                + "\n" + "bug testers to put OpenMMO through the gauntlet. If you are interested, check out "
                + "\n" + "our Discord Channel."
                + "\n"
                + "\n" + "In any case...come say hello on Discord!"
                , EditorStyles.helpBox);

            //CONTACT INFO
            EditorGUILayout.TextField("| CONNECT WITH US |", EditorStyles.largeLabel);
            EditorGUILayout.LabelField(
                "For all your OpenMMO questions, comments, and suggestions please join our Discord Server"
                , EditorStyles.helpBox);

            //DISCORD LINK
            if (GUILayout.Button("Discord Channel: " + "https://discord.gg/Cx9hAN4", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://discord.gg/Cx9hAN4");
            }

            //GETTING STARTED
            EditorGUILayout.TextField("| GETTING STARTED |", EditorStyles.largeLabel);
            EditorGUILayout.LabelField(
                "The best place to begin is with the Quickstart located in the Documentation folder."
                + "\n" + "Initial setup is fairly simple. One of our core values when we started OpenMMO was to "
                + "\n" + "maintain a fast and logical workflow so that developers could get straight to doing "
                + "\n" + "what developers do best right out of the box. To facilitate this goal we added a number "
                + "\n" + "of features not typically found in a basic game kit. From the automated build pipeline "
                + "\n" + "to a widget-based modular UI system, OpenMMO gives you the foundation and a few of the "
                + "\n" + "most crucial building blocks to build your online game upon."
                , EditorStyles.helpBox);
            
            //WRAPPING UP
            EditorGUILayout.TextField("| WRAPPING UP |", EditorStyles.largeLabel);
            EditorGUILayout.LabelField(
                "Delete HelloOpenMMO.cs from the OpenMMO/Documentation folder to disable this window."
                , EditorStyles.helpBox);
        }
    }
}
