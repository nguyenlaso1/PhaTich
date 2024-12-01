// @sonhg: class: EnemyAStar
using System;
using System.Collections;
using System.Collections.Generic;
using Bomb;
using SettlersEngine;
using UnityEngine;

public class EnemyAStar : MonoBehaviour
{
	private EnemyAStar.gridPosition GetRandomPos()
	{
		return new EnemyAStar.gridPosition(UnityEngine.Random.Range(0, this.Game.gridWidth - 1), UnityEngine.Random.Range(0, this.Game.gridHeight - 1));
	}

	private void Start()
	{
		this._botControllerScript = base.GetComponent<BotStupidController>();
		this.Init();
	}

	public void Init()
	{
		this.Game = Camera.main.GetComponent<GameManager>();
		this._mapControllerScripts = this.Game.mapController.GetComponent<MapController>();
		this.myColor = this.getRandomColor();
		EnemyAStar.gridPosition gridPosition = new EnemyAStar.gridPosition((int)base.transform.position.x, (int)base.transform.position.y);
		this.startGridPosition = new EnemyAStar.gridPosition(gridPosition.x, gridPosition.y);
		this.initializePosition();
	}

	private string FindDestinationPosToBomb(Vector2 _charPos, int _bombRange)
	{
		string result = string.Concat(new object[]
		{
			_charPos.x,
			"/",
			_charPos.y,
			"/",
			_charPos.x,
			"/",
			_charPos.y
		});
		EnemyAStar.RESULT result2 = EnemyAStar.RESULT.NotFound;
		int num = 2;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		while (result2 == EnemyAStar.RESULT.NotFound)
		{
			num2 = (int)_charPos.x + num;
			num3 = (int)_charPos.y + num;
			int num5 = (int)_charPos.x - num;
			num4 = (int)_charPos.y - num;
			if (num5 <= 0)
			{
				num5 = 0;
			}
			if (num4 <= 0)
			{
				num4 = 0;
			}
			if (num2 > 12)
			{
				num2 = 12;
			}
			if (num3 > 10)
			{
				num3 = 10;
			}
			for (int i = num5; i < num2 + 1; i++)
			{
				if (result2 == EnemyAStar.RESULT.NotFound)
				{
					for (int j = num4; j < num3 + 1; j++)
					{
						if (result2 == EnemyAStar.RESULT.NotFound)
						{
							Vector2 center = new Vector2((float)i, (float)j);
							if (!this.Game.grid[(int)center.x, (int)center.y].IsWall)
							{
								List<Vector2> points = this.FindAllPointAround(center, 1);
								List<Vector2> list = this.FindPointNextToDestroyableObj(points, 1, _charPos);
								foreach (Vector2 vector in list)
								{
									if (result2 == EnemyAStar.RESULT.NotFound)
									{
										List<Vector2> list2 = this.FindPlusArea(vector, _bombRange);
										List<Vector2> list3 = this.MovingAblePoints(vector, _bombRange + 1);
										foreach (Vector2 vector2 in list3)
										{
											if (result2 == EnemyAStar.RESULT.NotFound && !list2.Contains(vector2) && vector != vector2)
											{
												result2 = EnemyAStar.RESULT.found;
												result = string.Concat(new string[]
												{
													vector.x.ToString(),
													"/",
													vector.y.ToString(),
													"/",
													vector2.x.ToString(),
													"/",
													vector2.y.ToString()
												});
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (result2 == EnemyAStar.RESULT.NotFound)
			{
				num++;
			}
			if (result2 == EnemyAStar.RESULT.NotFound && num == 10)
			{
				result2 = EnemyAStar.RESULT.found;
				result = string.Concat(new string[]
				{
					_charPos.x.ToString(),
					"/",
					_charPos.y.ToString(),
					"/",
					_charPos.x.ToString(),
					"/",
					_charPos.y.ToString()
				});
			}
		}
		return result;
	}

	private List<Vector2> FindPointNextToDestroyableObj(List<Vector2> _points, int _range, Vector2 _charPos)
	{
		List<Vector2> list = new List<Vector2>();
		int num = 0;
		foreach (Vector2 vector in _points)
		{
			List<Vector2> list2 = this.FindPlusArea(vector, _range);
			if (list2 != null)
			{
				foreach (Vector2 vector2 in list2)
				{
					Transform transform = this.Game.mapHolder.transform.Find(vector2.x + "-" + vector2.y);
					if (transform != null && num < 3 && transform.gameObject.activeSelf && transform.tag == "Destroyable" && !this.Game.grid[(int)vector.x, (int)vector.y].IsWall && vector.x <= 12f && vector.y >= 0f && vector.x >= 0f && vector.y <= 10f && _charPos.x >= 0f && _charPos.y <= 10f && _charPos.x <= 12f && _charPos.y >= 0f && vector != _charPos)
					{
						EnemyAStar.MySolver<MyPathNode, object> mySolver = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
						IEnumerable<MyPathNode> enumerable = mySolver.Search(_charPos, vector, null);
						if (enumerable != null)
						{
							list.Add(vector);
							num++;
							if (num == 3)
							{
								break;
							}
						}
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Not found");
			}
		}
		return list;
	}

	private List<Vector2> FindPlusArea(Vector2 _center, int _range)
	{
		List<Vector2> list = new List<Vector2>();
		List<Vector2> list2 = this.FindAllPointAround(_center, _range);
		foreach (Vector2 item in list2)
		{
			if (item.x == _center.x || item.y == _center.y)
			{
				list.Add(item);
			}
		}
		return list;
	}

	private List<Vector2> MovingAblePoints(Vector2 _center, int _range)
	{
		List<Vector2> list = new List<Vector2>();
		List<Vector2> list2 = this.FindAllPointAround(_center, _range);
		int num = 0;
		foreach (Vector2 vector in list2)
		{
			if (vector.x >= 0f && vector.x <= 12f && vector.y >= 0f && vector.y <= 10f && _center.x >= 0f && _center.x <= 12f && _center.y <= 10f && _center.y >= 0f && num < 10)
			{
				EnemyAStar.MySolver<MyPathNode, object> mySolver = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
				IEnumerable<MyPathNode> enumerable = mySolver.Search(_center, vector, null);
				if (enumerable != null)
				{
					list.Add(vector);
					num++;
				}
			}
		}
		return list;
	}

	private List<Vector2> FindAllPointAround(Vector2 _center, int _range)
	{
		List<Vector2> list = new List<Vector2>();
		int num = (int)_center.x + _range;
		int num2 = (int)_center.y + _range;
		int num3 = (int)_center.x - _range;
		int num4 = (int)_center.y - _range;
		if (num3 <= 0)
		{
			num3 = 0;
		}
		if (num4 <= 0)
		{
			num4 = 0;
		}
		if (num > 12)
		{
			num = 12;
		}
		if (num2 > 10)
		{
			num2 = 10;
		}
		for (int i = num3; i < num + 1; i++)
		{
			for (int j = num4; j < num2 + 1; j++)
			{
				Vector2 item = new Vector2((float)i, (float)j);
				if (i >= 0 && i <= 12 && j >= 0 && j <= 10)
				{
					list.Add(item);
				}
			}
		}
		if (list.Count > 0)
		{
			return list;
		}
		return null;
	}

	public void findUpdatedPath(int currentX, int currentY)
	{
		EnemyAStar.MySolver<MyPathNode, object> mySolver = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
		IEnumerable<MyPathNode> enumerable = mySolver.Search(new Vector2((float)currentX, (float)currentY), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
		int num = 0;
		if (enumerable != null)
		{
			foreach (MyPathNode myPathNode in enumerable)
			{
				if (num == 1)
				{
					this.nextNode = myPathNode;
					break;
				}
				num++;
			}
		}
	}

	public IEnumerator MoveByAllPath(IEnumerable<MyPathNode> _path)
	{
		this._currentPath = _path;
		int x = 0;
		this.moveSpeed = this._botControllerScript.CurrentMoveSpeed;
		float _waitTime = 0.02f;
		float _range = 0.06f;
		if (_path != null)
		{
			foreach (MyPathNode node in _path)
			{
				this.input.x = 0f;
				this.input.y = 0f;
				if (x > 0)
				{
					this.isMoving = true;
					if (node.X > this.currentGridPosition.x)
					{
						this.input.x = 1f;
						this._currentMove = MoveDirection.RIGHT;
						while (base.transform.position.x < (float)node.X - _range)
						{
							yield return new WaitForSeconds(_waitTime);
						}
					}
					if (node.Y > this.currentGridPosition.y)
					{
						this.input.y = 1f;
						this._currentMove = MoveDirection.UP;
						while (base.transform.position.y < (float)node.Y - _range)
						{
							yield return new WaitForSeconds(_waitTime);
						}
					}
					if (node.Y < this.currentGridPosition.y)
					{
						this.input.y = -1f;
						this._currentMove = MoveDirection.DOWN;
						while (base.transform.position.y > (float)node.Y + _range)
						{
							yield return new WaitForSeconds(_waitTime);
						}
					}
					if (node.X < this.currentGridPosition.x)
					{
						this.input.x = -1f;
						this._currentMove = MoveDirection.LEFT;
						while (base.transform.position.x > (float)node.X + _range)
						{
							yield return new WaitForSeconds(_waitTime);
						}
					}
					base.transform.position = new Vector3((float)node.X, (float)node.Y, base.transform.position.z);
					this.currentGridPosition = new EnemyAStar.gridPosition(node.X, node.Y);
				}
				x++;
			}
			this._currentMove = MoveDirection.STAND;
		}
		yield break;
	}

	private void BombDetection()
	{
		List<Vector2> list = new List<Vector2>();
		EnemyAStar.gridPosition gridPosition = new EnemyAStar.gridPosition((int)base.transform.position.x, (int)base.transform.position.y);
		List<Vector2> list2 = this.MovingAblePoints(new Vector2((float)gridPosition.x, (float)gridPosition.y), 3);
		bool flag = false;
		foreach (BombModel bombModel in this._mapControllerScripts.GetBombList())
		{
			foreach (Vector2 item in this.FindPlusArea(new Vector2(bombModel.bomb.transform.position.x, bombModel.bomb.transform.position.y), bombModel.length))
			{
				list.Add(item);
			}
		}
		foreach (MyPathNode myPathNode in this._currentPath)
		{
			if (list.Contains(new Vector2((float)myPathNode.X, (float)myPathNode.Y)))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			List<Vector2> list3 = new List<Vector2>();
			foreach (Vector2 vector in list2)
			{
				if (!list.Contains(vector) && vector != new Vector2((float)gridPosition.x, (float)gridPosition.y))
				{
					list3.Add(vector);
				}
			}
			if (list3.Count > 0)
			{
				Vector2 vector2 = list3[UnityEngine.Random.Range(0, list3.Count)];
				this.SetUpPath(gridPosition, new EnemyAStar.gridPosition((int)vector2.x, (int)vector2.y));
				EnemyAStar.MySolver<MyPathNode, object> mySolver = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
				IEnumerable<MyPathNode> path = mySolver.Search(new Vector2((float)this.startGridPosition.x, (float)this.startGridPosition.y), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
				base.StopAllCoroutines();
				base.StartCoroutine(this.WaitToChangePos(path));
			}
		}
	}

	private IEnumerator WaitToChangePos(IEnumerable<MyPathNode> path)
	{
		this._currentMove = MoveDirection.STAND;
		base.StartCoroutine(this.MoveByAllPath(path));
		yield return new WaitForSeconds(3f);
		base.StartCoroutine(this.WaitToBreakAllBrick());
		yield break;
	}

	private Color getRandomColor()
	{
		Color result = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
		return result;
	}

	private void Update()
	{
		if (!this._botControllerScript.canMove)
		{
			this._botControllerScript.Move(this._currentMove);
		}
		if (this._mapControllerScripts.addedBomb)
		{
			this._mapControllerScripts.addedBomb = false;
			if (this._mapControllerScripts.GetBombList().Count > 0 && this._mapControllerScripts.GetBombList()[this._mapControllerScripts.GetBombList().Count - 1].userId != this._botControllerScript.userID)
			{
				this.BombDetection();
			}
		}
	}

	public Vector2 getGridPosition(int x, int y)
	{
		float num = this.Game.gridSize * 0.1f;
		float x2 = this.Game.gridBox.transform.position.x + this.Game.gridSize * (float)x - num;
		float y2 = this.Game.gridBox.transform.position.y + this.Game.gridSize * (float)y + num;
		return new Vector2(x2, y2);
	}

	public void initializePosition()
	{
		base.StartCoroutine(this.WaitToBreakAllBrick());
	}

	private IEnumerator WaitToBreakAllBrick()
	{
		int count = this._mapControllerScripts.destroyableObjectCount;
		int _bombRange = BombUserUtils.GetBombLength(this._botControllerScript.userID);
		while (count > 0 && !this._botControllerScript.isDead && !this.Game.mapController.GetComponent<GameController>().endGamePanel.gameObject.activeSelf)
		{
			if (this.canPlaceBomb)
			{
				_bombRange = BombUserUtils.GetBombLength(this._botControllerScript.userID);
				EnemyAStar.gridPosition _myPos = new EnemyAStar.gridPosition((int)base.transform.position.x, (int)base.transform.position.y);
				List<Vector2> _itemPosList = new List<Vector2>();
				List<Vector2> _currentMoveablePoints = this.MovingAblePoints(new Vector2((float)_myPos.x, (float)_myPos.y), 5);
				foreach (object obj in this._mapControllerScripts.itemHolder.transform)
				{
					Transform trans = (Transform)obj;
					if (trans.childCount > 0 && trans.GetChild(0).gameObject.activeSelf)
					{
						_itemPosList.Add(new Vector2((float)((int)trans.position.x), (float)((int)trans.position.y)));
					}
				}
				while (_itemPosList.Count > 0)
				{
					if (_currentMoveablePoints.Contains(_itemPosList[0]))
					{
						_myPos = new EnemyAStar.gridPosition((int)base.transform.position.x, (int)base.transform.position.y);
						this.SetUpPath(_myPos, new EnemyAStar.gridPosition((int)_itemPosList[0].x, (int)_itemPosList[0].y));
						EnemyAStar.MySolver<MyPathNode, object> aStar = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
						IEnumerable<MyPathNode> pathNext = aStar.Search(new Vector2((float)this.startGridPosition.x, (float)this.startGridPosition.y), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
						yield return base.StartCoroutine(this.MoveByAllPath(pathNext));
						yield return new WaitForSeconds(1f);
					}
					_itemPosList.RemoveAt(0);
				}
				_myPos = new EnemyAStar.gridPosition((int)base.transform.position.x, (int)base.transform.position.y);
				string[] posArr = this.FindDestinationPosToBomb(new Vector2((float)_myPos.x, (float)_myPos.y), _bombRange).Split(new char[]
				{
					'/'
				});
				this.SetUpPath(_myPos, new EnemyAStar.gridPosition(int.Parse(posArr[0]), int.Parse(posArr[1])));
				foreach (object obj2 in this.Game.mapHolder.transform)
				{
					Transform _trans = (Transform)obj2;
					string[] _pos = _trans.name.Split(new char[]
					{
						'-'
					});
					if (_trans.gameObject.activeSelf)
					{
						this.Game.addWall(int.Parse(_pos[0]), int.Parse(_pos[1]));
					}
					else
					{
						this.Game.removeWall(int.Parse(_pos[0]), int.Parse(_pos[1]));
					}
				}
				foreach (object obj3 in this.Game.bgHolder.transform)
				{
					Transform _trans2 = (Transform)obj3;
					if (_trans2.childCount > 0 && _trans2.GetChild(0).gameObject.activeSelf)
					{
						string[] _pos2 = _trans2.name.Split(new char[]
						{
							'-'
						});
						this.Game.addWall(int.Parse(_pos2[0]), int.Parse(_pos2[1]));
					}
				}
				if (this.endGridPosition.x - this.startGridPosition.x == 0 && this.endGridPosition.y - this.startGridPosition.y == 0)
				{
					List<Vector2> _currentPlusPoints = this.FindPlusArea(new Vector2((float)_myPos.x, (float)_myPos.y), _bombRange);
					List<Vector2> _moveAblePoints = this.MovingAblePoints(new Vector2((float)_myPos.x, (float)_myPos.y), _bombRange + 1);
					foreach (Vector2 _m in _moveAblePoints)
					{
						if (!_currentPlusPoints.Contains(_m) && _m != new Vector2((float)_myPos.x, (float)_myPos.y))
						{
							this.SetUpPath(_myPos, new EnemyAStar.gridPosition((int)_m.x, (int)_m.y));
							break;
						}
					}
					this._botControllerScript.PlaceBomb();
					this.canPlaceBomb = false;
					EnemyAStar.MySolver<MyPathNode, object> aStarNext = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
					IEnumerable<MyPathNode> pathNext2 = aStarNext.Search(new Vector2((float)this.startGridPosition.x, (float)this.startGridPosition.y), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
					yield return base.StartCoroutine(this.MoveByAllPath(pathNext2));
				}
				else
				{
					EnemyAStar.MySolver<MyPathNode, object> aStar2 = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
					IEnumerable<MyPathNode> path = aStar2.Search(new Vector2((float)this.startGridPosition.x, (float)this.startGridPosition.y), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
					yield return base.StartCoroutine(this.MoveByAllPath(path));
					_myPos = this.endGridPosition;
					this.SetUpPath(_myPos, new EnemyAStar.gridPosition(int.Parse(posArr[2]), int.Parse(posArr[3])));
					this._botControllerScript.PlaceBomb();
					this.canPlaceBomb = false;
					EnemyAStar.MySolver<MyPathNode, object> aStarNext2 = new EnemyAStar.MySolver<MyPathNode, object>(this.Game.grid);
					IEnumerable<MyPathNode> pathNext3 = aStarNext2.Search(new Vector2((float)this.startGridPosition.x, (float)this.startGridPosition.y), new Vector2((float)this.endGridPosition.x, (float)this.endGridPosition.y), null);
					yield return base.StartCoroutine(this.MoveByAllPath(pathNext3));
				}
			}
			yield return new WaitForSeconds(1.7f);
		}
		yield break;
	}

	public void SetUpPath(EnemyAStar.gridPosition _startPos, EnemyAStar.gridPosition _endPos)
	{
		this.startGridPosition = _startPos;
		this.currentGridPosition.x = this.startGridPosition.x;
		this.currentGridPosition.y = this.startGridPosition.y;
		this.endGridPosition = _endPos;
	}

	private IEnumerator WaitBomBlowUp(float _t)
	{
		this._isBomTrigger = false;
		yield return new WaitForSeconds(_t);
		this._isBomTrigger = true;
		yield break;
	}

	private GameManager Game;

	public MyPathNode nextNode;

	private bool gray;

	public MyPathNode[,] grid;

	private MoveDirection _currentMove = MoveDirection.STAND;

	public EnemyAStar.gridPosition currentGridPosition = new EnemyAStar.gridPosition();

	public EnemyAStar.gridPosition startGridPosition = new EnemyAStar.gridPosition();

	public EnemyAStar.gridPosition endGridPosition = new EnemyAStar.gridPosition();

	private EnemyAStar.Orientation gridOrientation = EnemyAStar.Orientation.Vertical;

	private bool allowDiagonals = true;

	private bool correctDiagonalSpeed = true;

	private Vector2 input;

	public bool isMoving;

	private Vector3 startPosition;

	private Vector3 endPosition;

	private float t;

	private bool _isBomTrigger;

	private float factor;

	private Color myColor;

	private BotStupidController _botControllerScript;

	private MapController _mapControllerScripts;

	public bool canPlaceBomb = true;

	private IEnumerable<MyPathNode> _currentPath;

	public float moveSpeed;

	private enum RESULT
	{
		found,
		NotFound
	}

	public class MySolver<TPathNode, TUserContext> : SpatialAStar<TPathNode, TUserContext> where TPathNode : IPathNode<TUserContext>
	{
		public MySolver(TPathNode[,] inGrid) : base(inGrid)
		{
		}

		protected double Heuristic(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			int distance = GameManager.distance;
			int num = Math.Abs(inStart.X - inEnd.X);
			int num2 = Math.Abs(inStart.Y - inEnd.Y);
			if (distance == 0)
			{
				return Math.Sqrt((double)(num * num + num2 * num2));
			}
			if (distance == 1)
			{
				return (double)(num * num + num2 * num2);
			}
			if (distance == 2)
			{
				return (double)Math.Min(num, num2);
			}
			if (distance == 3)
			{
				return (double)(num * num2 + (num + num2));
			}
			return (double)(Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y));
		}

		protected double NeighborDistance(SpatialAStar<TPathNode, TUserContext>.PathNode inStart, SpatialAStar<TPathNode, TUserContext>.PathNode inEnd)
		{
			return this.Heuristic(inStart, inEnd);
		}
	}

	public class gridPosition
	{
		public gridPosition()
		{
		}

		public gridPosition(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int x;

		public int y;
	}

	private enum Orientation
	{
		Horizontal,
		Vertical
	}
}
