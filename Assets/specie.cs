using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specie : MonoBehaviour {
	

	public int ebullitionTemp;
		 public string name;
		public specie(string specie, int temp){
			name = specie;
			ebullitionTemp = temp;
		}
}
