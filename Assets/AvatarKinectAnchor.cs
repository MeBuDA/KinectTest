using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kinect = Windows.Kinect;

public class AvatarKinectAnchor : MonoBehaviour
{
	public BodySourceManager bodySourceManager;
	public Animator avatar;
	[System.NonSerialized] bool flag = true;
	GameObject[] anchor = new GameObject[7];
	//Transform[] avatarBones = new Transform[15];
	void Start ()
	{
		int count = 0;
		foreach (Transform child in transform)
		{
			anchor[count] = child.gameObject;
			count++;
		}
		if (avatar == null)
		{
			Debug.Log ("アバター無いなった");
		}
		else
		{/*
			avatarBones[0] = avatar.GetBoneTransform (HumanBodyBones.Hips);
			avatarBones[1] = avatar.GetBoneTransform (HumanBodyBones.Spine);
			avatarBones[2] = avatar.GetBoneTransform (HumanBodyBones.Head);
			avatarBones[3] = avatar.GetBoneTransform (HumanBodyBones.LeftUpperArm);
			avatarBones[4] = avatar.GetBoneTransform (HumanBodyBones.LeftLowerArm);
			avatarBones[5] = avatar.GetBoneTransform (HumanBodyBones.LeftHand);
			avatarBones[6] = avatar.GetBoneTransform (HumanBodyBones.RightUpperArm);
			avatarBones[7] = avatar.GetBoneTransform (HumanBodyBones.RightLowerArm);
			avatarBones[8] = avatar.GetBoneTransform (HumanBodyBones.RightHand);
			avatarBones[9] = avatar.GetBoneTransform (HumanBodyBones.LeftUpperLeg);
			avatarBones[10] = avatar.GetBoneTransform (HumanBodyBones.LeftLowerLeg);
			avatarBones[11] = avatar.GetBoneTransform (HumanBodyBones.LeftFoot);
			avatarBones[12] = avatar.GetBoneTransform (HumanBodyBones.RightUpperLeg);
			avatarBones[13] = avatar.GetBoneTransform (HumanBodyBones.RightLowerLeg);
			avatarBones[14] = avatar.GetBoneTransform (HumanBodyBones.RightFoot);*/
		}
	}
	void Update ()
	{
		Kinect.Body body = bodySourceManager.GetData ().FirstOrDefault (b => b.IsTracked);
		if (body == null)
		{
			//Debug.Log ("体がないよ");
			return;
		}
		if (body.IsTracked)
		{
			if (flag)
			{
				CreateAnchor (body);
			}
			MoveAnchor (body);
		}
		if (!body.IsTracked)
		{
			flag = true;
		}

	}

	void CreateAnchor (Kinect.Body body)
	{
		this.gameObject.name = "Body:" + body.TrackingId;
		int i = 0;
		for (Kinect.JointType jt = Kinect.JointType.Head; jt <= Kinect.JointType.HandRight; jt++)
		{
			if ((int) jt == 2 | (int) jt == 7 | (int) jt == 11 | (int) jt == 15) { }
			else
			{
				anchor[i].name = jt.ToString ();
				i++;
			}
			flag = false;
		}
	}

	void OnAnimatorIK()
	{
		/*for (int i = 0; i < avatarBones.Length; i++)
		{
			//avatarBones[i].localPosition = anchor[i].transform.localPosition;
			//Debug.Log("test" + i);
			avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
			avatar.SetIKPosition(AvatarIKGoal.RightHand, rightHandAnchor.position);
			avatar.SetIKRotation(AvatarIKGoal.RightHand, rightHandAnchor.rotation);
		}*/
		avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot , 1);
		avatar.SetIKPosition(AvatarIKGoal.LeftFoot, anchor[9].transform.localPosition);
		avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot , 1);
		avatar.SetIKPosition(AvatarIKGoal.RightFoot, anchor[14].transform.localPosition);
		avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand , 1);
		avatar.SetIKPosition(AvatarIKGoal.LeftHand, anchor[5].transform.localPosition);
		avatar.SetIKPositionWeight(AvatarIKGoal.RightHand , 1);
		avatar.SetIKPosition(AvatarIKGoal.RightHand, anchor[8].transform.localPosition);
		Debug.Log("test");
	}

	void MoveAnchor (Kinect.Body body)
	{
		int i = 0;
		for (Kinect.JointType jt = Kinect.JointType.Head; jt <= Kinect.JointType.HandRight; jt++)
		{
			if ((int) jt == 2 | (int) jt == 6 | (int) jt == 10 | (int) jt == 15) { }
			else
			{
				anchor[i].transform.localPosition = GetVector3FromJoint (body.Joints[jt]);
				i++;
			}
		}
	}

	private static Vector3 GetVector3FromJoint (Kinect.Joint joint)
	{
		return new Vector3 (joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
	}

}