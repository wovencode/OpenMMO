//by Fhiz
using OpenMMO;
using OpenMMO.UI;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    /// <summary>
    /// This popup offers the user with choices: Confirm or Cancel. Both can trigger a unique
    /// action so you can use it for A/B decisions as well. This class is universal and can be
    /// used anywhere you require the user to make a Yes/No decision.
    /// </summary>
    [DisallowMultipleComponent]
    public partial class UIPopupPrompt : UIYesNoPrompt
    {
        public static UIPopupPrompt singleton;

        /// <summary>
        /// Awake sets the singleton (as this popup is unique) and calls base.Awake
        /// </summary>
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
        }
    }
}
