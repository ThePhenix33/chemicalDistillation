using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KMEdgework;
public class chemicalDistillation : MonoBehaviour {
	#region Module and Gameplay infos
	private static int _moduleIdCounter = 1;
	private int _moduleId = 0;
	private bool _isSolved = false, _lightsOn = false;
	#endregion

	public KMAudio Audio;
	public KMBombModule Module;
	public KMBombInfo Info;

	public MeshRenderer bSol, eSol, stain;
	private int baseSolution;
	private int[,]solPaths = {{7,6},{9,4},{2,1},{6,5},{8,2},{7,3},{1,2},{2,7},{3,4},{3,5},{2,8},{2,5},{4,6},{7,6},{4,2},{9,8},{9,7},{7,2},{1,2},{8,9},{1,2},{4,8},{5,9},{4,6},{3,2},{5,8},{3,8},{5,3},{4,8},{3,4}};
	private const int A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8, I=9, J=10, K=11, L=12, M=13, N=14, O=15, P=16, Q=17, R=18, S=19, T=20, U=21, V=22, W=23, X=24, Y=25, Z=26, S1=27, S2=28, S3=29, S4=30;
	private const int CYAN=1, ORANGE=2,BROWN=3,PINK=4,DBLUE=5,WHITE=6,RED=1,GREEN=2,BLUE=3,PURPLE=4,YELLOW=5;
	private int[,] baseSolutionTable = { { A, G, L, R, X, 0}, { B, H, M, S, Y, 0 }, { C, I, N, T, Z, 0 }, { D, J, O, U, S1, 0 }, { E, K, P, V, S2, 0 }, { F, L, Q, W, S3, 0 }, {0,0,0,0,0,0} };
	private int[] speciesTable = { 1, 10, 24, 6, 23, 25, 2, 17, 12, 15, 13, 7, 21, 5, 22, 3, 14, 19, 9, 11, 16, 8, 20, 4, 18 };

	private int[,] pathsA = {
		{20,15,11,16,17,21,22,23,18,13,12,7,6,10,5,0,1,2,3,4,9,8,14,19,24},{20,16,12,17,21,22,23,24,19,14,9,18,13,7,8,4,3,2,1,0,5,6,11,10,15},{6,11,5,0,1,2,3,7,12,16,10,15,20,21,22,23,24,17,18,19,13,14,8,9,5},
		{17,12,7,8,14,19,24,13,18,23,22,16,21,20,15,11,10,5,6,0,1,2,3,9,5},{20,21,22,23,24,19,14,9,4,3,2,1,0,5,10,15,16,17,18,13,8,7,6,11,12},{12,23,24,19,14,9,18,13,4,3,8,7,2,16,17,22,21,20,15,10,5,0,1,6,11},
		{9,18,11,8,12,13,4,3,7,2,6,1,0,5,16,10,15,20,17,21,22,23,14,19,24},{7,11,19,24,18,23,17,22,21,16,20,15,10,6,5,0,1,2,3,8,12,13,4,9,14},{10,11,12,13,15,20,21,22,23,16,17,24,18,19,14,8,9,4,3,7,6,5,2,1,0}};

	private int[,] pathsB = {
		{21,20,16,22,23,24,19,14,9,4,3,8,2,1,0,5,10,6,15,11,7,12,13,17,18},{13,17,11,7,12,8,9,14,18,19,24,23,22,21,20,15,16,10,6,5,0,1,2,3,4},{0,7,11,16,12,13,8,1,2,3,4,9,14,19,24,18,23,17,22,21,20,15,10,6,5},
		{12,21,22,23,24,19,14,8,9,4,3,2,1,0,5,10,15,20,16,11,6,7,13,18,17},{19,14,8,9,4,7,11,16,12,13,24,18,23,17,22,21,20,15,6,10,5,0,1,2,3},{9,14,18,19,24,23,17,13,4,8,12,11,22,21,20,15,16,10,5,6,0,1,2,7,3},
		{11,17,24,19,14,9,4,13,18,12,8,3,2,7,1,0,6,5,10,15,16,20,21,22,23},{22,23,24,19,18,17,16,11,12,13,14,9,8,7,6,4,3,2,1,0,5,10,15,20,21},{10,16,15,20,21,22,18,23,24,19,14,9,13,17,12,8,4,3,7,11,6,3,2,1,5}};

	private int[,] correctInequalities= {{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0},{0,0}};

	private Color32 orange = new Color32(255,50,0,255), brown = new Color32(139,69,19,255), pink = new Color32(255,192,203,255), purple = new Color32(128,0,128,255);
	private specie[] speciesList = {new specie ("Ledine", 96), new specie ("Resistoxyde", 100), new specie ("Wirol", 73), new specie ("Transistoryle", 67), new specie ("Buttonate", 76), new specie ("Arduinyle", 111), new specie("Russoxyle", 116), new specie ("Nolhane", 107), new specie ("Bessonate", 75), new specie ("Cameronyde", 54), new specie ("Jacksonol", 70), new specie ("Zemeckisate", 36), new specie ("Tysonate", 108), new specie ("Hawkyle", 94), new specie ("Einstale", 72), new specie ("Curyle", 113), new specie ("Teslade", 59), new specie ("Penrosyle", 79), new specie ("Albireol", 118), new specie ("Denebate", 31), new specie ("Altayle", 68), new specie ("Vegate", 43), new specie ("Aldebarane", 61), new specie ("Arcturusyle", 42), new specie ("Capellate", 91) };
	private specie firstSpecie, secondSpecie, thirdSpecie;

	private int hour;
	private int [] hours = {0,1,2,3,4,5,5,6,6,7,7,8,8,9,9,10,10,10,11,11,11,11,12,12,12,12,12,13,13,13,13,14,14,14,10,10,10,11,11,11,11,12,12,12,12,12,13,13,13,13,14,14,14,15,15,16,17,18,19,20,21,22,23};

	private int[] extractedSpecies = {  1, 1, 2, 1, 2,  1, 2, 1, 2, 1 ,  2, 1, 2, 1, 2 ,  2, 2, 2, 1, 1 ,  1, 1, 1, 2, 2  };
	public GameObject clockNeedle;
	public TextMesh phase;

	public GameObject notebook;

	public KMSelectable nextF, prevF, nextS, prevS, nextT, prevT, plusT, minusT, submit;
	public TextMesh first, second, third;
	private int textF=-1, textS=-1, textT=-1;

	private int temperature, userTemp=25;
	public TextMesh temp;

	private Color erlenColor;
	public KMSelectable erlenmeyer;
	private Color balloonColor;
	public KMSelectable balloon;
	private bool erlen = false;
	private bool ball = false;

	public TextMesh debug;
	// Use this for initialization
	void Start () {
		_moduleId = _moduleIdCounter++;
		Module.OnActivate += Activate;
	}

	private void Awake(){
		nextF.OnInteract += delegate() {
			handleNotebook(0);
			return false;
		};
		nextS.OnInteract += delegate() {
			handleNotebook(1);
			return false;
		};
		nextT.OnInteract += delegate() {
			handleNotebook(2);
			return false;
		};
		prevF.OnInteract += delegate() {
			handleNotebook(3);
			return false;
		};
		prevS.OnInteract += delegate() {
			handleNotebook(4);
			return false;
		};
		prevT.OnInteract += delegate() {
			handleNotebook(5);
			return false;
		};
		plusT.OnInteract += delegate() {
			handleTemperature(0);
			return false;
		};
		minusT.OnInteract += delegate() {
			handleTemperature(1);
			return false;
		};
		submit.OnInteract += delegate() {
			handleSubmit ();
			return false;
		};
		erlenmeyer.OnInteract += delegate() {
			handleErlenmeyer (0);
			return false;
		};
	
		balloon.OnInteract += delegate() {
			handleBalloon (0);
			return false;
		};

	}


	void Activate()
	{
		Init();
		_lightsOn = true;
	}

	void Init(){

		setNotebook ();
		setHour ();
		setStain ();
		setSolutions ();
		setSpecies ();
		setTable ();
		debug.text = firstSpecie.name + secondSpecie.name + thirdSpecie.name + temperature.ToString ();
	}


	void setTable(){


		for (int row = 0; row < 6; row++) {

			int[] temp = {
				baseSolutionTable [row, 0],
				baseSolutionTable [row, 1],
				baseSolutionTable [row, 2],
				baseSolutionTable [row, 3],
				baseSolutionTable [row, 4],
				0,
				0,
				0,
				0,
				0
			};
			int offset = Mathf.Abs (Info.GetIndicators ().Count ()); //ABCDE    off 3
			while (offset >= 5)
				offset -= 5;

			temp [9] = temp [9 - offset];
			temp [8] = temp [8 - offset];
			temp [7] = temp [7 - offset];
			temp [6] = temp [6 - offset];
			temp [5] = temp [5 - offset];

			if (offset <= 4)
				temp [4] = temp [4 - offset];
			if (offset <= 3)
				temp [3] = temp [3 - offset];
			if (offset <= 2)
				temp [2] = temp [2 - offset];
			if (offset <= 1)
				temp [1] = temp [1 - offset];

	


			for (int i = offset - 1; i >= 0; i--) {
				temp [i] = temp [5 + i];
			}


			for (int i = 0; i < 5; i++) {
				baseSolutionTable [row, i] = temp [i];
		//		Debug.LogFormat ("{0}", baseSolutionTable [row, i]);
			}
/*

		baseSolutionTable [o, offset] = temp [0]; // ABCAE
		baseSolutionTable [o, 1] = temp [offset]; // ADCAE

		baseSolutionTable [o, (offset+1>=5)?5-offset+1:offset+1] = temp [1]; // ADCAB
		baseSolutionTable [o, 2] = temp [offset+1]; //ADEAB

		baseSolutionTable [o, (offset+2>=5)?5-offset+2:offset+2] = temp [2]; // CDEAB
		
		}


		for(int j = 0;j<5;j++){
			Debug.LogFormat ("{0}", baseSolutionTable[0,j]);
		}
*/
		}
		}
	void setNotebook(){
		int angle = Random.Range (5, -30);
		notebook.transform.Rotate(new Vector3(0,angle,0));
		}

	void setHour(){
		hour = 	Shuffle (hours)[Random.Range (0, hours.Length - 1)];
		if (hour > 12) {
			clockNeedle.transform.Rotate (new Vector3 (0, 0, (360 * (hour - 12)) / 12));
		} else {
			clockNeedle.transform.Rotate (new Vector3 (0, 0, (360 * (hour)) / 12));
		}
		Debug.LogFormat ("[Chemical Distillation #{0}] Hour is {1}, {2} steps required then.", _moduleId, hour, (hour+2));
		if (hour >= 12) {
			phase.text = "PM";
		} else {
			phase.text = "AM";
		}
	}

	void setStain(){
		int stPresent = Random.Range (0, 3);

		if (stPresent != 0) {
			Debug.LogFormat ("[Chemical Distillation #{0}] The stain is absent", _moduleId);
			stain.enabled = false;
		} else {
			Debug.LogFormat ("[Chemical Distillation #{0}] The stain is present", _moduleId);
		}
	}

	void setSolutions(){
		int bSindex = Random.Range (1, 7);
		int eSindex = Random.Range (1, 6);

		#region Balloon Solution Color
		switch (bSindex) {
		case 1:
			bSol.material.color = Color.cyan;
			break;
		case 2:
			bSol.material.color = orange;
			break;
		case 3:
			bSol.material.color = brown;
			break;
		case 4:
			bSol.material.color = pink;
			break;
		case 5:
			bSol.material.color = Color.blue;
			break;
		case 6:
			bSol.material.color = Color.white;
			break;
		}
		#endregion
		#region Erlenmeyer Solution Color
		switch (eSindex) {
		case 1:
			eSol.material.color = Color.red;
			break;
		case 2:
			eSol.material.color = Color.green;
			break;
		case 3:
			eSol.material.color = Color.blue;
			break;
		case 4:
			eSol.material.color = purple;
			break;
		case 5:
			eSol.material.color = Color.yellow;
			break;		
		}
		#endregion
		erlenColor = eSol.material.color;
		balloonColor = bSol.material.color;
		baseSolution = baseSolutionTable [bSindex - 1, eSindex - 1];
		Debug.LogFormat ("[Chemical Distillation #{0}] Balloon is {1} " , _moduleId, bSindex);
		Debug.LogFormat ("[Chemical Distillation #{0}] Erlenmeyer is {1} " ,_moduleId,eSindex);
		Debug.LogFormat ("[Chemical Distillation #{0}] solution = {1}" , _moduleId,baseSolution);

	}
		
	void setSpecies(){
		int[] columnConditions = {Info.GetIndicators ().Count (), Info.GetBatteryCount (Battery.AA) + Info.GetBatteryCount (Battery.AAx3) + Info.GetBatteryCount (Battery.AAx4),  Info.GetSerialNumberNumbers().ToArray().ElementAt(1), Info.GetModuleNames().Count(), Info.GetSerialNumberNumbers().Last()};
		int[] rowConditions = {System.DateTime.Now.Day, System.DateTime.Now.Hour, System.DateTime.Now.Month, Info.GetSolvableModuleNames ().Count, Info.GetBatteryCount (Battery.D)};
		int[,] conditions = {{rowConditions[0]*columnConditions[0],rowConditions[0]*columnConditions[1],rowConditions[0]*columnConditions[2],rowConditions[0]*columnConditions[3],rowConditions[0]*columnConditions[4]},
			{rowConditions[1]*columnConditions[0],rowConditions[1]*columnConditions[1],rowConditions[1]*columnConditions[2],rowConditions[1]*columnConditions[3],rowConditions[1]*columnConditions[4]},
			{rowConditions[2]*columnConditions[0],rowConditions[2]*columnConditions[1],rowConditions[2]*columnConditions[2],rowConditions[2]*columnConditions[3],rowConditions[2]*columnConditions[4]},
			{rowConditions[3]*columnConditions[0],rowConditions[3]*columnConditions[1],rowConditions[3]*columnConditions[2],rowConditions[3]*columnConditions[3],rowConditions[3]*columnConditions[4]},
			{rowConditions[4]*columnConditions[0],rowConditions[4]*columnConditions[1],rowConditions[4]*columnConditions[2],rowConditions[4]*columnConditions[3],rowConditions[4]*columnConditions[4]}};
		int aCellValue = -1;
		int bCellValue = -1;
		int adding = 0;

		Debug.LogFormat ("[Chemical Distillation #{0}] Path A is {1}", _moduleId, (((solPaths [baseSolution-1,0])-1)+1));
		Debug.LogFormat ("[Chemical Distillation #{0}] Path B is {1}", _moduleId, (((solPaths [baseSolution-1,1])-1)+1));
		int test = 0;
		int compt = 0;
		int aCell=0;
		int bCell=0;
		int firstID = 0, secondID = 0, thirdID = 0;


		while(compt < (hour + 2)){
			Debug.LogFormat ("[Chemical Distillation #{0}] STEP {1}", _moduleId, (compt+1));
			#region A cells
			#region Row handle
			int row = 0;


			if (25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) <= 5) {
				row = 4;
			} else if (25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) <= 10 && 25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) >5) {
				row = 3;
			} else if (25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) <= 15&& 25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) >10) {
				row = 2;
			}	else if (25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) <= 20&& 25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) >15) {
				row = 1;
			} else if (25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) <= 25&& 25 - (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]) >20) {
				row = 0;
			}
			#endregion
			#region Column handle
			int column = (pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]);
			while (column >= 5) {
				column -= 5;
			}
			#endregion
			//Debug.LogFormat ("column is {0}", column);
			aCellValue = rowConditions [row] * columnConditions [column];
			//Debug.LogFormat ("Row conditions shows " + rowConditions [row-1]);
			//Debug.LogFormat ("Column conditions shows " + columnConditions [column]);
			Debug.LogFormat ("[Chemical Distillation #{0}] A Cell value is {1}", _moduleId, aCellValue);

			#endregion

			#region B cells
			#region Row B handle
			int rowB = 0;
			if (25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) <= 5) {
				rowB = 4;
			} else if (25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) <= 10 && 25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) > 5) {
				rowB = 3;
			} else if (25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) <= 15 && 25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt])> 10) {
				rowB = 2;
			} else if (25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) <= 20 && 25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) > 15) {
				rowB = 1;
			} else if (25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) <= 25 && 25 - (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]) > 20) {
				rowB = 0;
			}
			#endregion
			#region Column B handle
			int columnB = (pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]);
			while (columnB >= 5) {
				columnB -= 5;
			}
			#endregion
			//Debug.LogFormat ("column is {0}", column);
			bCellValue = rowConditions [rowB] * columnConditions [columnB];
			//Debug.LogFormat ("Row conditions shows " + rowConditions [row-1]);
			//Debug.LogFormat ("Column conditions shows " + columnConditions [column]);
			Debug.LogFormat ("[Chemical Distillation #{0}] B Cell value is {1}", _moduleId, bCellValue);
			#endregion

			if (Info.GetSerialNumberNumbers ().First() % 2 == 0) {

				if (aCellValue > bCellValue) {
					Debug.LogFormat ("[Chemical Distillation #{0}] Correct inequality because A>B", _moduleId);
					correctInequalities [test, 0] = speciesTable [pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]];
					correctInequalities [test, 1] = speciesTable [pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]];
					test++;
				}
			} else {
				if (aCellValue < bCellValue) {
					Debug.LogFormat ("[Chemical Distillation #{0}] Correct inequality because A<B", _moduleId);
					correctInequalities [test, 0] = speciesTable [pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]];
					correctInequalities [test, 1] = speciesTable [pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]];
					test++;
				}
			}  if (aCellValue==bCellValue){
				if (Info.GetOnIndicators().Contains("BOB")){Debug.LogFormat ("[Chemical Distillation #{0}] Corect inequality because BOB is lit", _moduleId);
					correctInequalities [test, 0] = speciesTable [pathsA [((solPaths [baseSolution - 1, 0]) - 1), compt]];
					correctInequalities [test, 1] = speciesTable [pathsB [((solPaths [baseSolution - 1, 1]) - 1), compt]];
					test++;
				}
				adding++;
			}
					
			compt++;

		} 
			//int row = pathsA[((solPaths[baseSolution-1,0])-1), test];
			//int column = pathsA[((solPaths[baseSolution-1,0])-1), test];
		Debug.LogFormat("[Chemical Distillation #{0}] There is {1} correct inequalities ", _moduleId, test);
		Debug.LogFormat("[Chemical Distillation #{0}] There is {1} equalities ", _moduleId, adding);


		if (test != 0) {
			
			aCell = correctInequalities [test - 1, 0];
			Debug.LogFormat ("gezr + " + aCell);	

			for (int incr = (test - 1) / 2; incr >= 0; incr--) {
				if (correctInequalities [incr, 1] < aCell) {
					bCell = correctInequalities [incr, 1];
					Debug.LogFormat ("bonjour + " + bCell);	
					break;
				} else if (incr == 0 && correctInequalities [incr, 1] >= aCell) {
					bCell = correctInequalities [incr, 1];
					Debug.LogFormat ("bonj2our + " + bCell);	
					break;
				}
			}
			if ((Mathf.Abs (aCell - bCell)) - 1 < 0) {
				firstID = Mathf.Abs (aCell - bCell);
				Debug.LogFormat ("Negative");
			} else {
				firstID = Mathf.Abs (aCell - bCell) - 1;
				Debug.LogFormat ("Positive");
			}

			firstID += adding;



			for (int decr = 0; decr < test-1; decr++) {	
			
				Debug.LogFormat ("decr " + decr);
				Debug.LogFormat ("firstCOr " + correctInequalities [decr, 0]);
				Debug.LogFormat ("secondCor " + correctInequalities [decr, 1]);
		
				Debug.LogFormat ("eAAREZGGR " + Mathf.Abs (aCell - bCell));

				if (speciesTable [correctInequalities [decr, 0] - 1] != speciesTable [firstID]) {
					secondID =	correctInequalities [decr, 0]-1;
				
					break;
				}
			}

			//When there is only 1 correct inequality, this happens (that's ugly but that's working)
	
			if (test == 1) {
				secondID =	correctInequalities [0, 0] - 1;
			}
		
				
			if(secondID==firstID){
				if (hour - 1 != firstID) {
					secondID = hour - 1;
				}else{
					secondID = hour + 2 - 1;
			}

			thirdID = Mathf.Abs (firstID - secondID)-1;

			if (thirdID == firstID || thirdID == secondID) {
				Debug.LogFormat ("double");
				thirdID = (firstID + secondID) / 2;
			}

			Debug.LogFormat ("First id is " + (firstID+1));
			Debug.LogFormat ("Second id is " + (secondID+1));
			Debug.LogFormat ("Third id is " + (thirdID+1));
				
			firstSpecie = speciesList [firstID];
			secondSpecie = speciesList [secondID];
			thirdSpecie = speciesList [thirdID];

			Debug.LogFormat ("First specie is " + firstSpecie.name);
			Debug.LogFormat ("Second specie is " + secondSpecie.name);
			Debug.LogFormat ("Third specie is " + thirdSpecie.name);
			//	Debug.LogFormat ("Row is " + row);.



			//C:\Users\Nanyboy\Documents\Unreal Projects\MyProject\Intermediate\Android\APK\gradle\app\build\tmp\expandedArchives\classes.jar_3q4nz8ua5pk0z64flsw70toic\com\google\protobuf
			//
			//Debug.LogFormat ("Path cell is " + speciesTable[pathsA[((solPaths[baseSolution-1,0])-1), test]]);
		
				setTemperature ();

		} else {
			if(Info.GetIndicatorCount()%2==0){
				ball = false;
			erlen = true;
				Debug.LogFormat ("Grief the notebook using the erlemeyer !");
			}else{
				erlen = false;
					ball=true;
				Debug.LogFormat ("Grief the notebook using the balloon !");
				}

		}
	}
		
	void setTemperature(){
		
		int cell = 0;
		for (int decr = 24; decr > 0; decr--) {	
			
			if (correctInequalities [decr, 0] != 0 && correctInequalities [decr, 1] != 0) {

				cell = speciesTable.ToList().IndexOf(correctInequalities [decr, 1]);
				break;
			}
		}
		Debug.LogFormat ("Cell at beggining is {0}", cell+1);
		if (stain.enabled == true) {
			cell += 5;
		}
		if (Info.GetSerialNumberNumbers ().Last() % 2 != 0) {
			cell -= 2;
		}
		if (Info.GetOnIndicators ().Count() > 0) {
			cell += 9;
		}
		if (speciesList.ToList().IndexOf (firstSpecie) % 2 == 0 || speciesList.ToList().IndexOf (secondSpecie) % 2 == 0 || speciesList.ToList().IndexOf (thirdSpecie) % 2 == 0) {
			cell += 1;
		}
		if("aeiouAEIOU".IndexOf(firstSpecie.name.First())>=0||"aeiouAEIOU".IndexOf(secondSpecie.name.First())>=0||"aeiouAEIOU".IndexOf(thirdSpecie.name.First())>=0){
			cell -=7;
		}

		if (cell >= 25) {
			cell -= 25;
		}
		if (cell < 0) {
			cell += 25;
		}
		Debug.LogFormat ("Cell at end is {0}", cell+1);
		int[] finalSpecies = { firstSpecie.ebullitionTemp, secondSpecie.ebullitionTemp, thirdSpecie.ebullitionTemp };
		finalSpecies = finalSpecies.OrderByDescending (c => c).ToArray ();
		switch (extractedSpecies [cell]) {
		case 1:
			Debug.LogFormat ("There is 1 specie in the erlenmeyer");
			for (int i = finalSpecies [2]; i < finalSpecies [1]; i++) {
				if (i ==finalSpecies[2]+Info.GetPortCount()) {
					temperature = i;
					break;
				}
				if (i == finalSpecies [1] - 1) {
					temperature = finalSpecies [2] + 1;			//If the loop is ended before the first condition is reached, this part avoids the temperature to be 0
					break;
				}
			}
			break;
		case 2:
			Debug.LogFormat ("There are 2 species in the erlenmeyer");
			for (int i = finalSpecies [1]; i < finalSpecies [0]; i++) {
				if (i ==finalSpecies[1]+Info.GetPortCount()) {
					temperature = i;			
					break;
				}
				if (i == finalSpecies [0] - 1) {	
					temperature = finalSpecies [1] + 1;			//If the loop is ended before the first condition is reached, this part avoids the temperature to be 0
					break;
				}
			}
			break;
		}

		Debug.LogFormat ("The temperature was " + temperature);
	}
	// Update is called once per frame
	void handleNotebook(int text){

		writingSound ();
		switch(text){
		case 0:
			textF++;
			if (textF == 25) {
				textF = 0;
			}
			first.text = speciesList [textF].name;

			break;
		case 1:
			textS++;
			if (textS == 25) {
				textS = 0;
			}
			second.text = speciesList [textS].name;
			break;
		case 2:
			textT++;
			if (textT == 25) {
				textT = 0;
			}
			third.text = speciesList [textT].name;
			break;
		case 3:
			textF--;
			if (textF == -1||textF ==-2) {
				textF = 24;
			}
			first.text = speciesList [textF].name;
			break;
		case 4:
			textS--;
			if (textS == -1||textS ==-2) {
				textS = 24;
			}
			second.text = speciesList [textS].name;
			break;
		case 5:
			textT--;
			if (textT == -1||textT ==-2) {
				textT = 24;
			}
			third.text = speciesList [textT].name;
			break;
	}
	}

	void handleTemperature (int value){
		switch (value) {
		case 0:
			userTemp++;
		
			break;
		case 1:
			userTemp--;
			break;
		}
		temp.text = userTemp.ToString ();
	}

	void handleSubmit(){
		Debug.LogFormat("[Chemical Distillation #{0}] You submitted {1} as first specie, {2} as second and {3} as third, at the temperature {4} °C ", _moduleId, first.text, second.text, third.text, temp.text);
		if (!erlen && !ball && firstSpecie.name == first.text && secondSpecie.name == second.text && thirdSpecie.name == third.text &&userTemp==temperature) {
			Module.HandlePass ();
		} else {
			Module.HandleStrike ();
		}
	}

	void handleErlenmeyer(int phase){
		Debug.LogFormat("[Chemical Distillation #{0}] You used the erlenmeyer's solution !", _moduleId);

		switch (phase) {
		case 0:
			if (erlen) {
				Module.HandlePass ();
			} else {
				Module.HandleStrike ();
			}
			break;
		case 1:
			eSol.material.color = orange;
			break;
		case 2:
			eSol.material.color = erlenColor;
			break;
		}
	}

	void handleBalloon(int phase){
		Debug.LogFormat("[Chemical Distillation #{0}] You used the balloon's solution !", _moduleId);

		switch (phase) {
		case 0:
			if (ball) {
				Module.HandlePass ();
			} else {
				Module.HandleStrike ();
			}
			break;
		case 1:
			bSol.material.color = orange;
			break;
		case 2:
			bSol.material.color = balloonColor;
			break;
		}
	}

	void writingSound(){
		int sound = Random.Range (0, 7);

		switch (sound) {
		case 0:
			Audio.PlaySoundAtTransform ("pencil0", notebook.transform);
			break;
		case 1:
			Audio.PlaySoundAtTransform ("pencil1", notebook.transform);
			break;
		case 2:
			Audio.PlaySoundAtTransform ("pencil2", notebook.transform);
			break;
		case 3:
			Audio.PlaySoundAtTransform ("pencil3", notebook.transform);
			break;
		case 4:
			Audio.PlaySoundAtTransform ("pencil4", notebook.transform);
			break;
		case 5:
			Audio.PlaySoundAtTransform ("pencil5", notebook.transform);
			break;
		case 6:
			Audio.PlaySoundAtTransform ("pencil6", notebook.transform);
			break;
		case 7:
			Audio.PlaySoundAtTransform ("pencil7", notebook.transform);
			break;
		}
	}
	void Update () {
		}

	public T Shuffle<T>(T list) where T : IList
	{
		if (list == null)
			throw new System.ArgumentNullException("list");
		for (int j = list.Count; j >= 1; j--)
		{
			int item = Random.Range(0, j);
			if (item < j - 1)
			{
				var t = list[item];
				list[item] = list[j - 1];
				list[j - 1] = t;
			}
		}
		return list;
	}
		
}
