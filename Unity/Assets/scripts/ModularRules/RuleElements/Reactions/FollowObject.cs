﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowObject : Reaction
{
	public float FollowSpeed = 10;

	public GameObject FixedToObject = null;
	public Vector3 Offset = Vector3.zero;

	public bool StayBehindObject = false;

	private Transform targetTransform;

	private ActorDropDown actorDropDown;

	RuleGenerator generator;

	public override void Initialize(RuleGenerator generator)
	{
		base.Initialize(generator);

		this.generator = generator;

		if (FixedToObject)
		{
			Reactor.transform.parent = FixedToObject.transform;
			Reactor.transform.localPosition = Offset;
		}

		// gui stuff
		string[] actors = generator.Gui.ActorNames;
		actorDropDown = new ActorDropDown(
			System.Array.FindIndex(actors, item => item == Reactor.Label),
			actors,
			ref generator.Gui.OnAddedActor, ref generator.Gui.OnRenamedActor, ref generator.Gui.OnDeletedActor);
	}

	public override RuleData GetRuleInformation()
	{
		ReactionData rule = base.GetRuleInformation() as ReactionData;

		rule.parameters = new List<Param>();
		rule.parameters.Add(new Param()
		{
			name = "FollowSpeed",
			type = FollowSpeed.GetType(),
			value = FollowSpeed
		});
		rule.parameters.Add(new Param() 
		{ 
			name = "Offset",
			type = Offset.GetType(),
			value = Offset.x + " " + Offset.y + " " + Offset.z
		});
		if (FixedToObject != null)
		{
			rule.parameters.Add(new Param()
			{
				name = "FixedToObject",
				type = FixedToObject.GetComponent(typeof(Actor)).GetType(),
				value = (FixedToObject.GetComponent(typeof(Actor)) as Actor).Id
			});
		}
		else
		{
			rule.parameters.Add(new Param()
			{
				name = "FixedToObject",
				type = typeof(Actor),
				value = -1
			});

		}
		rule.parameters.Add(new Param()
		{
			name = "StayBehindObject",
			type = StayBehindObject.GetType(),
			value = StayBehindObject
		});

		return rule;
	}

	public override void ShowGui(RuleData ruleData)
	{
		int resultIndex = actorDropDown.Draw();
		if (resultIndex > -1)
		{
			int resultId = generator.Gui.GetActorDataByLabel(actorDropDown.Content[resultIndex].text).id;
			(ruleData as ReactionData).actorId = resultId;
			if (resultId != Reactor.Id)
				generator.ChangeActor(this, resultId);
		}

		GUILayout.Label("follows with an offset of", RuleGUI.ruleLabelStyle);

		Offset = RuleGUI.ShowParameter(Offset, "followOffset" + Reactor.Id);
		ChangeParameter("Offset", ruleData.parameters, Offset);

		GUILayout.Label("and a speed of", RuleGUI.ruleLabelStyle);

		FollowSpeed = RuleGUI.ShowParameter(FollowSpeed);
		ChangeParameter("FollowSpeed", ruleData.parameters, FollowSpeed);

		GUILayout.Label("Always stay behind the target?", RuleGUI.ruleLabelStyle);
		StayBehindObject = RuleGUI.ShowParameter(StayBehindObject);

		ChangeParameter("StayBehindObject", ruleData.parameters, StayBehindObject);
	}

	void OnEnable()
	{
		Register();
	}

	void OnDisable()
	{
		Unregister();
	}

	protected override void React(GameEventData eventData)
	{
		DataPiece targetObject = eventData.Get(EventDataKeys.TargetObject);
		if (targetObject != null)
		{
			GameObject target = targetObject.data as GameObject;

			if (target != null)
			{
				targetTransform = target.transform;
			}
		}
	}

	void FixedUpdate()
	{
		if (!FixedToObject && targetTransform != null && 
			Vector3.Distance(targetTransform.position + Offset, Reactor.transform.position) > 0.5f)
		{
			if (!StayBehindObject)
			{
				if (Reactor.rigidbody != null)
					Reactor.rigidbody.AddForce(((targetTransform.position + Offset) - Reactor.transform.position).normalized * FollowSpeed);
				else
					Reactor.transform.position = Vector3.Lerp(Reactor.transform.position, targetTransform.position + Offset, Time.deltaTime * FollowSpeed);
			}
			else
			{
				if (Reactor.rigidbody != null)
				{
					Reactor.rigidbody.AddForce(
						((targetTransform.position + 
							targetTransform.forward * Offset.z + 
							targetTransform.right * Offset.x + 
							targetTransform.up * Offset.y) - 
							Reactor.transform.position).normalized * FollowSpeed);
				}
				else
				{
					Reactor.transform.position = Vector3.Lerp(
						Reactor.transform.position,
						targetTransform.position + targetTransform.forward * Offset.z + targetTransform.right * Offset.x + targetTransform.up * Offset.y,
						Time.deltaTime * FollowSpeed);
				}
			}
		}

		if (targetTransform != null)
			Reactor.transform.LookAt(targetTransform.position);
	}
}
