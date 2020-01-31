
using Wovencode;
using Wovencode.DebugManager;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Wovencode.DebugManager
{
	
	// ===================================================================================
	// DebugProfile
	// ===================================================================================
	public partial class DebugProfile
	{
		
		public string name;
		public Stopwatch stopWatch = new Stopwatch();
		
		protected List<long> operations = new List<long>();
		protected long lastOperation;
		
		protected bool active;
		
		// -------------------------------------------------------------------------------
		// DebugProfile (Constructor)
		// -------------------------------------------------------------------------------
		public DebugProfile(string _name)
		{
			name = _name;
			Restart();
		}
		
		// -------------------------------------------------------------------------------
		// Stop
		// -------------------------------------------------------------------------------
		public void Stop()
		{
			stopWatch.Stop();
			lastOperation = stopWatch.ElapsedMilliseconds;
			operations.Add(lastOperation);
			active = false;
		}
		
		// -------------------------------------------------------------------------------
		// Restart
		// -------------------------------------------------------------------------------
		public void Restart()
		{
			lastOperation = 0;
			stopWatch.Stop();
			Start();
		}
		
		// -------------------------------------------------------------------------------
		// Reset
		// -------------------------------------------------------------------------------
		public void Reset(string _name="")
		{
			
			if (!String.IsNullOrWhiteSpace(_name))
				name = _name;
				
			operations.Clear();
			lastOperation = 0;
			stopWatch.Stop();
		}
		
		// -------------------------------------------------------------------------------
		// Print
		// -------------------------------------------------------------------------------
		public string Print
		{
			get
			{
				CheckActive();
				return "[DebugProfile] '"+name+"' ~"+lastOperation.ToString()+"ms (~"+GetAverage.ToString()+"ms average)";
			}
		}
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		protected void Start()
		{
			stopWatch.Start();
			active = true;
		}
		
		// -------------------------------------------------------------------------------
		// CheckActive
		// -------------------------------------------------------------------------------
		protected void CheckActive()
		{
			if (active)
				Stop();
		}
		
		// -------------------------------------------------------------------------------
		// GetAverage
		// -------------------------------------------------------------------------------
		protected long GetAverage
		{
			get {
				long average = 0;
				foreach (long operation in operations)
					average += operation;
				average /= operations.Count;
				return average;
			}
		}
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================