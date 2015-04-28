﻿using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	private Individual I;
	private Color baseColor;
	public bool selected = false;

	public float floorOffset = 1;
	public float speed = 5;
	public float stopDistanceOffset = 0.5f;

	private bool selectedByClick = false;

	private float angle;
	private GameObject target;
	private Vector3 newPosition;
	private float timeTaken;
	private float duration = 40.0f;

	void Start () 
	{
		I = gameObject.GetComponentInParent<Individual>();
		newPosition = transform.position;
	}

	private void Update ()
	{
		if (GetComponent<Renderer>().isVisible && Input.GetMouseButton(0)) 
		{
			if(!selectedByClick)
			{
				Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
				camPos.y = CameraOperator.InvertMouseY(camPos.y);
				selected = CameraOperator.selection.Contains(camPos);

				if(selected)
				{
					I.isSelectioned = true;
				}
				else
				{
					I.isSelectioned = false;
				}
			}
		}

		if(selected && Input.GetMouseButtonDown(1))		//Si sélectionné et clic droit
		{
			target = GameObject.FindGameObjectWithTag("target");
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				newPosition = hit.point;
				newPosition.y = 1;
				target.transform.position = newPosition;	//Object target prend position du clic
				I.target = newPosition;
				I.gotDest = true;		//Objet possède une destination

				/*if(I.target.x >= transform.position.x && I.target.z >= transform.position.z)
					angle = 270f + Mathf.Tan((I.target.x - transform.position.x)/(I.target.z - transform.position.z)) * Mathf.Rad2Deg;
				if(I.target.x > transform.position.x && I.target.z < transform.position.z)
					angle = Mathf.Tan((I.target.x - transform.position.x)/(transform.position.z - I.target.z)) * Mathf.Rad2Deg;
				if(I.target.x <= transform.position.x && I.target.z <= transform.position.z)
					angle = 180f - Mathf.Tan((transform.position.x - I.target.x)/(transform.position.z - I.target.z)) * Mathf.Rad2Deg;
				if(I.target.x < transform.position.x && I.target.z > transform.position.z)
					angle = 270f - Mathf.Tan((transform.position.x - I.target.x)/(I.target.z - transform.position.z)) * Mathf.Rad2Deg;
				//transform.localEulerAngles.z = angle;*/
				//transform.Rotate(0, 0, angle);
			}	//Definit angle, pour que la cellule regarde vers la target
		}

		if(I.gotDest)
		{
			if((transform.position.x - I.target.x >= -2 && transform.position.x - I.target.x <= 2) && (transform.position.z - I.target.z >= -2 && transform.position.z - I.target.z <= 2))	//Gérer pour supprimer dest quand cells dans rayon autour de la target.
			{
				I.gotDest = false;		//Plus de destination car elle a été atteinte
				Debug.Log("Destination atteinte");
			}
			transform.position = Vector3.Lerp(transform.position, I.target, 1/(duration*(Vector3.Distance(transform.position, I.target))));
		}
	}
	
	private void OnMouseDown()
	{
		if (I.isPlayed)
		{
			I.isSelectioned = !I.isSelectioned;
			if (I.isSelectioned) 
			{
				selected = true;
				selectedByClick = true;
			}
			else
			{
				selected = false;
				I.isSelectioned = false;
			}
		}
	}

	private void OnMouseUp()
	{
		if (selectedByClick) 
		{
			selected = true;
			I.isSelectioned = true;
		}
		else 
		{
			selected = false;
			I.isSelectioned = false;
		}
		selectedByClick = false;
	}
}
