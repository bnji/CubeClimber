using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IUsable
{
    void Use(UnityEngine.Object sender, RaycastHit raycastHit);
}