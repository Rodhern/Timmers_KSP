﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KeepFit
{
    class RosterWindow : KeepFitInfoWindow
    {
        private Vector2 scrollPosition;
        private bool showAvailable;
        private bool showAssigned;

        public RosterWindow()
            : base(true, true, "roster")
        {
            this.WindowCaption = "KeepFit Roster";
            this.Hide();
            this.DragEnabled = true;

            this.WindowRect = new Rect(0, 0, 300, 300);
        }

        protected override void FillWindow(int id)
        {
            GameConfig gameConfig = scenarioModule.GetGameConfig();
            if (!gameConfig.enabled)
            {
                GUILayout.Label(new GUIContent("DISABLED"), uiResources.styleBarTextRed);
                return;
            }

            if (gameConfig.roster == null)
            {
                return;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            GUILayout.BeginVertical();
            if (gameConfig.roster.available != null)
            {
                DrawRoster(id, "Available Crew", gameConfig.roster.available.crew.Values, ref showAvailable);
            }
            if (gameConfig.roster.assigned != null)
            {
                DrawRoster(id, "Assigned Crew", gameConfig.roster.assigned.crew.Values, ref showAssigned);
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void DrawRoster(int windowHandle, string title, ICollection<KeepFitCrewMember> crew, ref bool expanded)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title);
            GUILayout.FlexibleSpace();
            if (DrawChevron(expanded))
            {
                expanded = !expanded;
            }
            GUILayout.EndHorizontal();

            if (expanded)
            {
                // indent crew listing
                GUILayout.BeginHorizontal();
                GUILayout.Space(4);
                GUILayout.BeginVertical();
                DrawCrew(windowHandle, crew, false, true);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        private GUIStyle getGeeAccumStyle(KeepFitCrewMember crew, Period period, GeeLoadingAccumulator accum)
        {
            GameConfig gameConfig = scenarioModule.GetGameConfig();

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.green;
            style.wordWrap = false;

            GeeToleranceConfig tolerance = gameConfig.GetGeeTolerance(period);
            if (tolerance == null)
            {
                return style;
            }

            float geeWarn = GeeLoadingCalculator.GetFitnessModifiedGeeTolerance(tolerance.warn, crew, gameConfig);
            float geeFatal = GeeLoadingCalculator.GetFitnessModifiedGeeTolerance(tolerance.fatal, crew, gameConfig);

            float gee = accum.GetLastGeeMeanPerSecond();
            if (gee > geeFatal)
            {
                style.normal.textColor = Color.red;
            }
            else
            {
                if (gee > geeWarn)
                {
                    style.normal.textColor = Color.yellow;
                }
            }

            return style;
        }
    }
}
