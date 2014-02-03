﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace KeepFit
{
    /// <summary>
    /// Part added to modules (command modules?) to allow them to interact with KeepFit directly
    /// </summary>
    public class KeepFitPartModule  : PartModule
    {
        private Rect windowPosition = new Rect();

        // Values from the .cfg file
        [KSPField(isPersistant = false, guiActive = true, guiName="KeepFit Activity Level")]
        public string strActivityLevel = ActivityLevel.UNKNOWN.ToString();

        internal ActivityLevel activityLevel;


        public override void OnAwake()
        {
            this.Log_DebugOnly("Awake", "Part[{0}] strActivityLevel[{1}]", this.name, this.strActivityLevel);

            if (strActivityLevel != null)
            {
                activityLevel = ActivityLevel.COMFY;
            }
            else
            {
                try
                {
                    activityLevel = (ActivityLevel)Enum.Parse(typeof(ActivityLevel), strActivityLevel);
                }
                catch (ArgumentException)
                {
                    this.Log_DebugOnly("Awake", "Part[{0}] strActivityLevel[{1}] is not a valid ActivityLevel", this.name, this.strActivityLevel);
                }
            }
        }

        /// <summary>
        /// Called when the part is started by Unity.
        /// </summary>
        public override void OnStart(StartState state)
        {
            print("KeepFitPartModule::OnStart");

            base.OnStart(state);


            if (!HighLogic.LoadedSceneIsFlight)
            {
                print("KeepFitPartModule::OnStart - Not in flight scene - bailing");
                return;
            }   

            // TDXX - I think this check is redundant
            if (state != StartState.Editor)
            {
                RenderingManager.AddToPostDrawQueue(0, OnDraw);
            }
         
            // perform any initialisation steps
            //  - ? list the kerbals in this vessel
            //  - ? setup vessel kerbals KeepFit status UI?
        }

        public void Update()
        {
            //print("KeepFitPartModule::Update");

            if (!HighLogic.LoadedSceneIsFlight)
            {
                print("KeepFitPartModule::Update - Not in flight scene - bailing");
                return;
            }
        }

        public void Destroy()
        {
            print("KeepFitPartModule::Destroy");
        }

        private void OnDraw()
        {
            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                windowPosition = GUILayout.Window(1945, windowPosition, OnWindow, "Kerbal KeepFit");
            }
        }

        private void OnWindow(int windowId)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(250f));
            GUILayout.Label("This is a label");
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }
    }
}
