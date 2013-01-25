//-----------------------------------------------------------------------
// <copyright file="AllJoynClientServer.cs" company="Qualcomm Innovation Center, Inc.">
// Copyright 2012, Qualcomm Innovation Center, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using UnityEngine;
using AllJoynUnity;
using basic_clientserver;
using System.Collections;

namespace client_server
{
	public class AllJoynClientServer : MonoBehaviour
	{
		
		private string msgText = "";
		private bool isWorking = false;
		private string playerNick = "";
		
		private static int BUTTON_SIZE = 75;
		private long spamCount = 0;
		
		private bool spamMessages = false;
		
		private int playerNr;
		
		void OnGUI ()
		{
			if (!isWorking)
				return;
			
			if(BasicChat.chatText != null){
				GUI.TextArea(new Rect (0, 0, Screen.width, (Screen.height / 2)), BasicChat.chatText);
			}
			int i = 0;
			int xStart = (Screen.height / 2)+10+((i++)*BUTTON_SIZE);
			
			if(BasicChat.AllJoynStarted) {
				if(GUI.Button(new Rect(0,xStart,(Screen.width)/3, BUTTON_SIZE),"STOP ALLJOYN"))
				{	
					basicChat.CloseDown();
				}
			}
			
			if(BasicChat.currentJoinedSession != null) {
				if(GUI.Button(new Rect(((Screen.width)/3),xStart,(Screen.width)/3, BUTTON_SIZE),
					"Leave \n"+BasicChat.currentJoinedSession.Substring(BasicChat.currentJoinedSession.LastIndexOf("."))))
				{	
					basicChat.LeaveSession();
				}
			}
			
			if(!BasicChat.AllJoynStarted) {
				if(GUI.Button(new Rect(((Screen.width)/3)*2,xStart,(Screen.width)/3, BUTTON_SIZE), "START ALLJOYN"))
				{	
					basicChat.StartUp();
				}
			}
			
			foreach(string name in BasicChat.sFoundName){
				xStart = (Screen.height / 2)+10+((i++)*BUTTON_SIZE);
				if(GUI.Button(new Rect(10,xStart,(Screen.width-20), BUTTON_SIZE),name.Substring(name.LastIndexOf("."))))
				{	
					basicChat.JoinSession(name);
					playerNr = 2;
				}
			}
			
			msgText = GUI.TextField(new Rect (0, Screen.height-BUTTON_SIZE, (Screen.width/4) * 3, BUTTON_SIZE), msgText);
			if(GUI.Button(new Rect(Screen.width - (Screen.width/4),Screen.height-BUTTON_SIZE, (Screen.width/4), BUTTON_SIZE),"Send"))
			{	
				ArrayList points = new ArrayList();
				points.Add(new Vector3(1.0f, 2.0f, 3.0f));
				points.Add(new Vector3(-1.0f, 0.0f, 200.0f));
				points.Add(new Vector3(-93.2f, 23.99f, 200.99f));
				basicChat.SendVector(points);
				//basicChat.SendTheMsg(msgText);
				//Debug easter egg
				if(string.Compare("spam",msgText) == 0)
					spamMessages = true;
				else if(string.Compare("stop",msgText) == 0)
				{
					spamMessages = false;
					spamCount = 0;
				}
			}
		}
		
		// Use this for initialization
		void Start()
		{
			DontDestroyOnLoad(this);
			//basicChat = new BasicChat();
		}
	
		// Update is called once per frame
	    void Update()
		{
			if (!isWorking)
				return;
	        if (Input.GetKeyDown(KeyCode.Escape)) {
				basicChat.CloseDown();
				Application.Quit();
			}
			if(spamMessages) {
				basicChat.SendTheMsg("("+(spamCount++)+") Spam: "+System.DateTime.Today.Millisecond);
			}
		}
		
		public void Init(string nick)
		{
			isWorking = true;
			playerNick = nick;
			playerNr = 1;
			Debug.Log("Starting up AllJoyn service and client");
			basicChat = new BasicChat(playerNick);
		}
		
		public void UpdateState(double[] state)
		{
			Debug.Log ("AllJoynClientServer: SHOULD sent data to P2");	
		}
		
		public int GetPlayerNr() {
			return playerNr;	
		}
	
		BasicChat basicChat;
	}
}