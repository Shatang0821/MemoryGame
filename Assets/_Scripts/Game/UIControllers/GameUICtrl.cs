using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork.Utils;
using System.Collections.Generic;

 
public class GameUICtrl : UICtrl {

	public override void Awake() {

		base.Awake();
		this.gameObject.SetActive(false);
	}

	void Start() {
	}

}
