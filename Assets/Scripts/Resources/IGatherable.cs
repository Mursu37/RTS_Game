using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGatherable
{
   Resource ResourceType { get; set; }
   int Gather();
}
