
using OpenMMO;
using OpenMMO.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateLoader : MonoBehaviour
{
	
	/*
	
		This is just a example to demonstrate how template loading and caching works
		
		In a real project you DO NOT require the TemplateLoader, as the templates are
		simply loaded into memory the first time they are accessed.
	
		Add this script to a GameObject in your scene and hit Play, then take a look
		at the log.
		
	*/
	
	[Header("EXAMPLE ONLY - You don't need the template loader")]
	public string templateNameFoo 	= "FooTemplate1";
	public string templateNameBar 	= "BarTemplate1";
	
	[Header("Debug Mode")]
	public DebugHelper debug = new DebugHelper();
	
	// -----------------------------------------------------------------------------------
	// Awake
	// -----------------------------------------------------------------------------------
    void Awake()
    {
    	if (debug.debugMode)
    	{
    		LoadFoo();
    		LoadBar();
    		LoadSingleton();
    	}
    }
    
	// -----------------------------------------------------------------------------------
	// LoadSpells
	// This example triggers caching of templates by accessing one of it by its name
	// -----------------------------------------------------------------------------------
	void LoadFoo()
	{
	
		debug.Log("----- LOAD FOO -----");
		debug.Log("Trying to load all FooTemplates by accessing one by name...");
    	
    	debug.StartProfile("LoadFoo");
    	
    	FooTemplate.data.TryGetValue(templateNameFoo.GetDeterministicHashCode(), out FooTemplate template);
    	
    	debug.StopProfile("LoadFoo");
    	
        if (template)
        	debug.Log("The FooTemplate '"+template.title+"' was found in the 'FooTemplate' Dictionary");
        else
        	debug.Log("No FooTemplate named '"+templateNameFoo+"' was found!");
        
        debug.Log("A total of '"+FooTemplate.data.Count+"' FooTemplates have been cached from '"+FooTemplate._folderName+"' into the Dictionary.");
		
		debug.PrintProfile("LoadFoo");
		
	}
	
	// -----------------------------------------------------------------------------------
	// LoadBar
	// This example uses the 'BuildCache' method on the template class to cache them
	// -----------------------------------------------------------------------------------
	void LoadBar()
	{
	
		debug.Log("----- LOAD BAR -----");
		debug.Log("Trying to load all BarTemplates by using 'BuildCache'...");
    	
    	debug.StartProfile("LoadBar");
    	
    	BarTemplate.BuildCache();
    	
    	debug.StopProfile("LoadBar");
    	
        debug.Log("A total of '"+BarTemplate.data.Count+"' BarTemplates have been cached from '"+BarTemplate._folderName+"' into the Dictionary.");
	
		debug.PrintProfile("LoadBar");
		
	}
	
	// -----------------------------------------------------------------------------------
	// LoadSingleton
	// 
	// -----------------------------------------------------------------------------------
	void LoadSingleton()
	{
		debug.Log("----- LOAD GAME RULES -----");
		debug.Log("Trying to access GameRulesTemplate via Singleton...");
		debug.Log("maxPlayersPerUser: "+GameRulesTemplate.singleton.maxPlayersPerUser);
		debug.Log("maxUsersPerDevice: "+GameRulesTemplate.singleton.maxUsersPerDevice);
		debug.Log("maxUsersPerEmail: "+GameRulesTemplate.singleton.maxUsersPerEmail);
	}
	
	// -----------------------------------------------------------------------------------
	
}
