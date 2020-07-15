
using System;
using System.Text;
using UnityEngine;
using OpenMMO;

namespace OpenMMO {

	public partial class Constants
	{
		
		
		/*
			Names of defines used to indicate CLIENT or SERVER mode
		*/
		
		public const string BuildModeServer 				= "_SERVER";
    	public const string BuildModeClient 				= "_CLIENT";

		/*
			Names of variables saved in player preferences:
		*/
		
		public const string PlayerPrefsUserName 			= "UserName";
		public const string PlayerPrefsPassword 			= "UserPass";	
		public const string PlayerPrefsLastServer 			= "LastServer";
		
		/*
			Names of variables saved in editor preferences:
		*/
		
		public const string EditorPrefsMySQLAddress			= "mySQLAddress";
		public const string EditorPrefsMySQLPort			= "mySQLPort";
		public const string EditorPrefsMySQLUsername		= "mySQLUsername";
		public const string EditorPrefsMySQLPassword		= "mySQLPassword";
		public const string EditorPrefsMySQLDatabase		= "mySQLDatabase";
		public const string EditorPrefsMySQLCharset			= "mySQLCharset";
				
	}
	
}