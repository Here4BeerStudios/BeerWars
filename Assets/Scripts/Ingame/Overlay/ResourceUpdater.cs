using System;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	public class ResourceUpdater : MonoBehaviour
	{
		public ResourceHandler resources;

		public Text corn;
		public Text beer;

		public void Update() {
			corn.text = resources.Corn.ToString ();
			beer.text = resources.Beer.ToString ();
		}
	}
}

