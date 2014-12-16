using UnityEngine;
using System.Collections;

public interface IDestroyable
{
		bool IsDestroyable { get; set; }
		void Destroy ();
}
