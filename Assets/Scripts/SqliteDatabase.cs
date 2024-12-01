// @sonhg: class: SqliteDatabase
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class SqliteDatabase
{
	public SqliteDatabase(string dbName)
	{
		this.pathDB = Path.Combine(Application.persistentDataPath, dbName);
		string text = Path.Combine(Application.streamingAssetsPath, dbName);
		if (!File.Exists(this.pathDB) || File.GetLastWriteTimeUtc(text) > File.GetLastWriteTimeUtc(this.pathDB))
		{
			if (text.Contains("://"))
			{
				WWW www = new WWW(text);
				while (!www.isDone)
				{
				}
				if (string.IsNullOrEmpty(www.error))
				{
					File.WriteAllBytes(this.pathDB, www.bytes);
				}
				else
				{
					this.CanExQuery = false;
				}
			}
			else if (File.Exists(text))
			{
				File.Copy(text, this.pathDB, true);
			}
			else
			{
				this.CanExQuery = false;
				UnityEngine.Debug.Log("ERROR: the file DB named " + dbName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
			}
		}
	}

	[DllImport("sqlite3")]
	private static extern int sqlite3_open(string filename, out IntPtr db);

	[DllImport("sqlite3")]
	private static extern int sqlite3_close(IntPtr db);

	[DllImport("sqlite3")]
	private static extern int sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);

	[DllImport("sqlite3")]
	private static extern int sqlite3_step(IntPtr stmHandle);

	[DllImport("sqlite3")]
	private static extern int sqlite3_finalize(IntPtr stmHandle);

	[DllImport("sqlite3")]
	private static extern IntPtr sqlite3_errmsg(IntPtr db);

	[DllImport("sqlite3")]
	private static extern int sqlite3_column_count(IntPtr stmHandle);

	[DllImport("sqlite3")]
	private static extern IntPtr sqlite3_column_name(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern IntPtr sqlite3_column_text(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern IntPtr sqlite3_column_blob(IntPtr stmHandle, int iCol);

	[DllImport("sqlite3")]
	private static extern int sqlite3_column_bytes(IntPtr stmHandle, int iCol);

	private bool IsConnectionOpen { get; set; }

	private void Open()
	{
		this.Open(this.pathDB);
	}

	private void Open(string path)
	{
		if (this.IsConnectionOpen)
		{
			throw new SqliteException("There is already an open connection");
		}
		if (SqliteDatabase.sqlite3_open(path, out this._connection) != 0)
		{
			throw new SqliteException("Could not open database file: " + path);
		}
		this.IsConnectionOpen = true;
	}

	private void Close()
	{
		if (this.IsConnectionOpen)
		{
			SqliteDatabase.sqlite3_close(this._connection);
		}
		this.IsConnectionOpen = false;
	}

	public void ExecuteNonQuery(string query)
	{
		if (!this.CanExQuery)
		{
			UnityEngine.Debug.Log("ERROR: Can't execute the query, verify DB origin file");
			return;
		}
		this.Open();
		if (!this.IsConnectionOpen)
		{
			throw new SqliteException("SQLite database is not open.");
		}
		IntPtr stmHandle = this.Prepare(query);
		if (SqliteDatabase.sqlite3_step(stmHandle) != 101)
		{
			throw new SqliteException("Could not execute SQL statement.");
		}
		this.Finalize(stmHandle);
		this.Close();
	}

	public DataTable ExecuteQuery(string query)
	{
		if (!this.CanExQuery)
		{
			UnityEngine.Debug.Log("ERROR: Can't execute the query, verify DB origin file");
			return null;
		}
		this.Open();
		if (!this.IsConnectionOpen)
		{
			throw new SqliteException("SQLite database is not open.");
		}
		IntPtr stmHandle = this.Prepare(query);
		int num = SqliteDatabase.sqlite3_column_count(stmHandle);
		DataTable dataTable = new DataTable();
		for (int i = 0; i < num; i++)
		{
			string item = Marshal.PtrToStringAnsi(SqliteDatabase.sqlite3_column_name(stmHandle, i));
			dataTable.Columns.Add(item);
		}
		while (SqliteDatabase.sqlite3_step(stmHandle) == 100)
		{
			object[] array = new object[num];
			for (int j = 0; j < num; j++)
			{
				switch (SqliteDatabase.sqlite3_column_type(stmHandle, j))
				{
				case 1:
					array[j] = SqliteDatabase.sqlite3_column_int(stmHandle, j);
					break;
				case 2:
					array[j] = SqliteDatabase.sqlite3_column_double(stmHandle, j);
					break;
				case 3:
				{
					IntPtr ptr = SqliteDatabase.sqlite3_column_text(stmHandle, j);
					array[j] = Marshal.PtrToStringAnsi(ptr);
					break;
				}
				case 4:
				{
					IntPtr source = SqliteDatabase.sqlite3_column_blob(stmHandle, j);
					int num2 = SqliteDatabase.sqlite3_column_bytes(stmHandle, j);
					byte[] array2 = new byte[num2];
					Marshal.Copy(source, array2, 0, num2);
					array[j] = array2;
					break;
				}
				case 5:
					array[j] = null;
					break;
				}
			}
			dataTable.AddRow(array);
		}
		this.Finalize(stmHandle);
		this.Close();
		return dataTable;
	}

	public void ExecuteScript(string script)
	{
		string[] array = script.Split(new char[]
		{
			';'
		});
		foreach (string text in array)
		{
			if (!string.IsNullOrEmpty(text.Trim()))
			{
				this.ExecuteNonQuery(text);
			}
		}
	}

	private IntPtr Prepare(string query)
	{
		IntPtr result;
		if (SqliteDatabase.sqlite3_prepare_v2(this._connection, query, query.Length, out result, IntPtr.Zero) != 0)
		{
			IntPtr ptr = SqliteDatabase.sqlite3_errmsg(this._connection);
			throw new SqliteException(Marshal.PtrToStringAnsi(ptr));
		}
		return result;
	}

	private void Finalize(IntPtr stmHandle)
	{
		if (SqliteDatabase.sqlite3_finalize(stmHandle) != 0)
		{
			throw new SqliteException("Could not finalize SQL statement.");
		}
	}

	private const int SQLITE_OK = 0;

	private const int SQLITE_ROW = 100;

	private const int SQLITE_DONE = 101;

	private const int SQLITE_INTEGER = 1;

	private const int SQLITE_FLOAT = 2;

	private const int SQLITE_TEXT = 3;

	private const int SQLITE_BLOB = 4;

	private const int SQLITE_NULL = 5;

	private bool CanExQuery = true;

	private IntPtr _connection;

	private string pathDB;
}
