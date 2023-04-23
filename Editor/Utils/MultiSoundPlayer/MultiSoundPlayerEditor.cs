﻿using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSoundPlayer
{
    [CustomEditor(typeof(MultiSoundPlayer))]
    public class MultiSoundPlayerEditor : Editor
    {
        private MultiSoundPlayer _multiSoundPlayer;

        private SerializedProperty _audioSource;
        private SerializedProperty _sounds;
        private SerializedProperty _playbackVoices;
        private SerializedProperty _paused;
        private SerializedProperty _currentVoice;
        
        private int _soundIndexToPlay;

        private void OnEnable()
        {
            _multiSoundPlayer = (MultiSoundPlayer) target;
            
            _audioSource = serializedObject.FindProperty("_audioSource");
            _sounds = serializedObject.FindProperty("_sounds");
            _playbackVoices = serializedObject.FindProperty("_playbackVoices");
            _paused = serializedObject.FindProperty("_paused");
            _currentVoice = serializedObject.FindProperty("_currentVoice");
        }

        public override void OnInspectorGUI()
        {
            if (_multiSoundPlayer == null)
            {
                return;
            }

            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.PropertyField(_audioSource);
            EditorGUILayout.PropertyField(_sounds);
            EditorGUILayout.Space(20);
            
            var labelGuiStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            EditorGUILayout.LabelField("Properties", labelGuiStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(_playbackVoices);
            EditorGUILayout.PropertyField(_currentVoice);
            EditorGUILayout.PropertyField(_paused);
            EditorGUILayout.Space(20);

            #region PLAY SOUND DEBUG
            EditorGUILayout.LabelField("Play Sound Debug", labelGuiStyle, GUILayout.ExpandWidth(true));

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Sound can be played only during runtime", MessageType.Warning);
                GUI.enabled = false;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play", GUILayout.MinWidth(50), GUILayout.ExpandWidth(true)))
            {
                _multiSoundPlayer.PlaySingleSound(_soundIndexToPlay);
            }
            
            GUI.enabled = true;
            
            _soundIndexToPlay = EditorGUILayout.IntField(_soundIndexToPlay, GUILayout.MinWidth(50), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            #endregion
            
            EditorGUILayout.EndVertical();
            
            // apply modified properties and repaint
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                InternalEditorUtility.RepaintAllViews();
            }
        }
    }
}