using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class playSound : MonoBehaviour 
{

	public AudioSource open;

	public AudioSource close;

	public AudioSource click;

	public AudioSource click2;

	public AudioSource inspect;

	public AudioSource clue;

	public AudioSource select;

	public AudioSource CCaio;

	public AudioSource CCoughing;

	public AudioSource CDamn;

	public AudioSource CEh;

	public AudioSource CHum;

	public AudioSource CMake;

	public AudioSource CNah;

	public AudioSource CPisciare;

	public AudioSource CSalt;

	public AudioSource CUh;

	public AudioSource CYea;

	public AudioSource CAyy;

	public AudioSource FCertainly;

	public AudioSource FClearing;

	public AudioSource FDeep;

	public AudioSource FFarewill;

	public AudioSource FFine;

	public AudioSource FHmm;

	public AudioSource FNo;

	public AudioSource FRequisitions;

	public AudioSource FWell;

	public AudioSource FWhat;

	public AudioSource FWhatever;

	public AudioSource FWith;

	public AudioSource MAnkyat;

	public AudioSource MHello;

	public AudioSource MMhmm;

	public AudioSource MMumbling;

	public AudioSource MNo;

	public AudioSource MO;

	public AudioSource MOf;

	public AudioSource MPerhaps;

	public AudioSource MPshh;

	public AudioSource MSekhat;

	public AudioSource MTake;

	public AudioSource MYes;

	public AudioSource PAs;
	
	public AudioSource PDont;

	public AudioSource PHey;

	public AudioSource PPshh;

	public AudioSource PSee;

	public AudioSource PThe;

	public AudioSource PThroat;

	public AudioSource PUgh;

	public AudioSource PUmmm;

	public AudioSource PWait;

	public AudioSource PYea;

	public AudioSource PYup;

	public AudioSource ECough;

	public AudioSource EKay;

	public AudioSource EMhm;

	public AudioSource EScrew;

	public AudioSource EThis;

	public AudioSource EUgh;

	public AudioSource EUgn;

	public AudioSource EUh;

	public AudioSource EUhhh;

	public AudioSource EWhatever;

	public AudioSource EWhats;

	public AudioSource EWhere;

	public void Open()
	{

		open.Play ();

	}

	public void Close()
	{

		close.Play ();

	}

	public void Click()
	{

		click.Play ();

	}

	public void Click_Two()
	{

		click2.Play ();

	}

	public void Inspect()
	{


		inspect.Play ();

	}

	public void Clue()
	{

		clue.Play ();

	}

	public void Select()
	{

		select.Play ();

	}

	[YarnCommand("Speak")]
	public void Speak(string line)
	{

		switch (line)
		{
			
			case "CCaio":

				CCaio.Play();

				break;
			
			case "CCoughing":

				CCoughing.Play();

				break;
			
			case "CDamn":

				CDamn.Play();

				break;
			
			case "CEh":

				CEh.Play();

				break;
			
			case "CHum":

				CHum.Play();

				break;
			
			case "CMake":

				CMake.Play();

				break;
			
			case "CNah":

				CNah.Play();

				break;
			
			case "CPisciare":

				CPisciare.Play();

				break;
			
			case "CSalt":

				CSalt.Play();

				break;
			
			case "CUh":

				CUh.Play();

				break;
			
			case "CYea":

				CYea.Play();

				break;
			
			case "CAyy":

				CAyy.Play();

				break;
			
			case "FCertainly":
				
				FCertainly.Play();

				break;
			
			case "FClearing":
				
				FClearing.Play();

				break;
			
			case "FDeep":
				
				FDeep.Play();

				break;
			
			case "FFarewill":
				
				FFarewill.Play();

				break;
			
			case "FFine":
				
				FFine.Play();

				break;
			
			case "FHmm":
				
				FHmm.Play();

				break;
			
			case "FNo":
				
				FNo.Play();

				break;
			
			case "FRequisitions":
				
				FRequisitions.Play();

				break;
			
			case "FWell":
				
				FWell.Play();

				break;
			
			case "FWhat":
				
				FWhat.Play();

				break;
			
			case "FWhatever":
				
				FWhatever.Play();

				break;
			
			case "FWith":
				
				FWith.Play();

				break;
			
			case "MAnkyat":
				
				MAnkyat.Play();

				break;
			
			case "MHello":
				
				MHello.Play();

				break;
			
			case "MMhmm":
				
				MMhmm.Play();

				break;
			
			case "MMumbling":
				
				MMumbling.Play();

				break;
			
			case "MNo":
				
				MNo.Play();

				break;
			
			case "MOf":
				
				MOf.Play();

				break;
			
			case "MPerhaps":
				
				MPerhaps.Play();

				break;
			
			case "MPshh":
				
				MPshh.Play();

				break;
			
			case "MSekhat":
				
				MSekhat.Play();

				break;
			
			case "MTake":
				
				MTake.Play();

				break;
			
			case "MYes":
				
				MYes.Play();

				break;
			
			case "PAs":
				
				PAs.Play();

				break;
			
			case "PDont":
				
				PDont.Play();

				break;
			
			case "PHey":
				
				PHey.Play();

				break;
			
			case "PPshh":
				
				PPshh.Play();

				break;
			
			case "PSee":
				
				PSee.Play();

				break;
			
			case "PThe":
				
				PThe.Play();

				break;
			
			case "PThroat":
				
				PThroat.Play();

				break;
			
			case "PUgh":
				
				PUgh.Play();

				break;
			
			case "PUmmm":
				
				PUmmm.Play();

				break;
			
			case "PWait":
				
				PWait.Play();

				break;
			
			case "PYea":
				
				PYea.Play();

				break;
			
			case "PYup":
				
				PYup.Play();

				break;
			
			case "ECough":
				
				ECough.Play();

				break;
			
			case "EKay":
				
				EKay.Play();

				break;
			
			case "EMhm":
				
				EMhm.Play();

				break;
			
			case "EScrew":
				
				EScrew.Play();

				break;
			
			case "EThis":
				
				EThis.Play();

				break;
			
			case "EUgh":
				
				EUgh.Play();

				break;
			
			case "EUgn":
				
				EUgn.Play();

				break;
			
			case "EUh":
				
				EUh.Play();

				break;
			
			case "EUhhh":
				
				EUhhh.Play();

				break;
			
			case "EWhatever":
				
				EWhatever.Play();

				break;
			
			case "EWhats":
				
				EWhats.Play();

				break;
			
			case "EWhere":
				
				EWhere.Play();

				break;
			
		}
		
	}

}