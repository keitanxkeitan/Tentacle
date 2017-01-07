using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

	// 腕パーツのリスト
	private List<GameObject> armParts = new List<GameObject>();

	// 経過フレーム
	private int totalFrm = 0;

	// 回転周期
	public int rotateCycle = 60;

	// IK
	public bool isIK = false;

	// IK の目標
	public Transform ikTarget;

	// CCD アルゴリズム反復回数
	public int ccdIterNum = 1;

	// Use this for initialization
	void Start () {
		// 腕パーツのリストを構築
		findArmPartsRec (this.gameObject, ref armParts);
	}
	
	// Update is called once per frame
	void Update () {
		if (isIK) {
			// CCD アルゴリズムで IK
			if (armParts.Count >= 1) {
				GameObject armPart_Last = armParts [armParts.Count - 1];

				for (int i = 0; i < ccdIterNum; ++i) {
					// 末端から根本へイテレートする
					for (int iArmPart = armParts.Count - 1; iArmPart >= 0; --iArmPart) {
						GameObject armPart = armParts [iArmPart];

						// エフェクタ位置
						Vector3 posEffect = armPart_Last.transform.position + armPart_Last.transform.forward * 5.0f;
						// -MEMO- 値を決め打ちにしているが本当はロケータを置くのがよい

						// エフェクタ方向
						Vector3 dirEffect;
						{
							dirEffect = posEffect - armPart.transform.position;
							dirEffect.Normalize ();
						}

						// 目標方向
						Vector3 dirTarget;
						{
							dirTarget = ikTarget.position - armPart.transform.position;
							dirTarget.Normalize ();
						}

						// 回転角
						float rotateRad;
						{
							float dot = Vector3.Dot (dirEffect, dirTarget);
							rotateRad = Mathf.Acos (dot);
						}

						if (rotateRad > 0.001f) {
							// 回転軸
							Vector3 rotateAxis = Vector3.Cross (dirEffect, dirTarget);
							rotateAxis.Normalize ();

							// 回転
							armPart.transform.Rotate (rotateAxis, rotateRad * Mathf.Rad2Deg);
						}
					}
				}
			}
		} else {
			// 腕パーツを回転させる
			foreach (GameObject armPart in armParts) {
				float rotateDir = ((totalFrm / rotateCycle) % 2 == 0) ? +1.0f : -1.0f; // 回転周期で回転方向を変える
				armPart.transform.Rotate (rotateDir * Vector3.right);
			}
		}

		// 経過フレームの更新
		++totalFrm;
	}

	/**
	 * 子要素を再帰的にたどりながら腕パーツを探す
	 */
	void findArmPartsRec( GameObject obj, ref List<GameObject> armParts ) {
		Transform children = obj.GetComponentInChildren<Transform> ();

		if (children.childCount == 0) {
			return;
		}

		foreach (Transform child in children) {
			if (child.name.Equals ("ArmPart")) {
				armParts.Add (child.gameObject);
				findArmPartsRec (child.gameObject, ref armParts);
			}
		}
	}
}
