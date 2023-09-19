using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public class EditorDatabase : MonoBehaviour
{
    private string conn;

    void Start()
    {
        Debug.Log("Connecting to database...");
        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=3D_Modular_Gaming_Tool;";

        SqlConnection dbconn = new SqlConnection(conn);

        try
        {
            dbconn.Open();
            Debug.Log("Connected to database!");
        }
        catch (System.Exception)
        {
            Debug.Log("Failed to connect to database!");
            throw;
        }
        finally
        {
            dbconn.Close();
        }
    }

    public void SaveLevel(LevelEditor level, string name)
    {
        Debug.Log("Saving level " + name + " to database...");

        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        //check if level table exists
        /*using (SqlCommand command = new SqlCommand("SELECT * FROM sys.tables WHERE name = 'Level'", dbconn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Debug.Log("Level table does not exist, creating table...");
                    CreateLevelTables(dbconn);


                }
            }
        }*/

        List<CreatedObject.Data> createdObjects = level.createdObjects;
        List<FloorData.Data> floors = level.floors;

        //if level in database update, else add
        using (SqlCommand commandSelect = new SqlCommand("SELECT * FROM Level WHERE levelname = @levelname", dbconn))
        {
            commandSelect.Parameters.AddWithValue("@levelname", name);
            using (SqlDataReader reader = commandSelect.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Debug.Log("Level " + name + " already exists in database, updating...");

                    reader.Close();

                    DeleteLevel(name, dbconn);

                }


                reader.Close();
                //Save level

                using (SqlCommand command = new SqlCommand("INSERT INTO Level (levelname) VALUES (@levelname)", dbconn))
                {
                    command.Parameters.AddWithValue("@levelname", name);
                    command.ExecuteNonQuery();
                }

                //Save objects
                foreach (CreatedObject.Data obj in createdObjects)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO Object (type, position, rotation, scale, level) VALUES (@type, @position, @rotation, @scale, @level)", dbconn))
                    {
                        command.Parameters.AddWithValue("@type", obj.tag);
                        command.Parameters.AddWithValue("@position", obj.position.ToString());
                        command.Parameters.AddWithValue("@rotation", obj.rotation.ToString());
                        command.Parameters.AddWithValue("@scale", obj.scale.ToString());
                        command.Parameters.AddWithValue("@level", name);
                        command.ExecuteNonQuery();
                    }
                }

                //Save floors

                foreach (FloorData.Data floor in floors)
                {

                    if (!string.IsNullOrEmpty(floor.floorPlanPath))
                    {
                        Debug.Log("Path : " + floor.floorPlanPath);

                        using (SqlCommand command = new SqlCommand("INSERT INTO Floor (floorNumber, floorplan, level) VALUES (@floorNumber, @floorplan, @level)", dbconn))
                        {
                            command.Parameters.AddWithValue("@floorNumber", floor.floorNumber);
                            command.Parameters.AddWithValue("@floorplan", floor.floorPlanPath);
                            command.Parameters.AddWithValue("@level", name);
                            command.ExecuteNonQuery();
                        }

                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Floor (floorNumber, level) VALUES (@floorNumber, @level)", dbconn))
                        {
                            command.Parameters.AddWithValue("@floorNumber", floor.floorNumber);
                            command.Parameters.AddWithValue("@level", name);
                            command.ExecuteNonQuery();
                        }
                    }
                }

            }


        }

        dbconn.Close();
    }

    /*void CreateLevelTables(SqlConnection dbconn)
    {
        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Level(LevelName VARCHAR (50) NOT NULL, TimeLimit INT NULL, MinTime INT NULL, CONSTRAINT PK_Level PRIMARY KEY CLUSTERED(LevelName ASC)", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Object(ObjectID INT IDENTITY (1, 1) NOT NULL,Type VARCHAR (50) NOT NULL, Position VARCHAR (50) NOT NULL,Rotation VARCHAR (50) NOT NULL,Scale VARCHAR (50) NOT NULL,Level    VARCHAR (50) NOT NULL,CONSTRAINT PK_Object PRIMARY KEY CLUSTERED ([ObjectID] ASC),CONSTRAINT FK_LevelName FOREIGN KEY (Level) REFERENCES Level (LevelName) ON DELETE CASCADE;", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Floor (FloorLevel INT NOT NULL, Floorplan  VARCHAR (MAX) NULL,Level      VARCHAR (50)  NOT NULL, CONSTRAINT PK_Floor PRIMARY KEY CLUSTERED (FloorLevel ASC, Level ASC), CONSTRAINT FK_LevelNameFloor FOREIGN KEY (Level) REFERENCES Level (LevelName) ON DELETE CASCADE;", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

    }*/

    public LevelEditor LoadLevel(string name)
    {
        Debug.Log("Loading level " + name + " from database...");

        LevelEditor level = new LevelEditor();
        level.createdObjects = new List<CreatedObject.Data>();
        level.floors = new List<FloorData.Data>();
        
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    int maxTime = reader.GetInt32(reader.GetOrdinal("maximumTime"));
                    int minTime = reader.GetInt32(reader.GetOrdinal("minimumTime"));
                    Debug.Log("Max time: " + maxTime + " Min time: " + minTime);

                    reader.Close();

                    if (GameObject.Find("ScoreCanvas") != null)
                    {
                        // Call AddTimers function here using the maxTime and minTime values
                        CountdownTimer countdown = GameObject.Find("ScoreCanvas").GetComponent<CountdownTimer>();
                        if (countdown != null)
                        {
                            countdown.setTimers(maxTime, minTime);
                            Debug.Log("Timers set");
                        }
                    }
                    
                    

                    // Load objects
                    using (SqlCommand objCommand = new SqlCommand("SELECT * FROM Object WHERE level = @levelname", dbconn))
                    {
                        objCommand.Parameters.AddWithValue("@levelname", name);
                        using (SqlDataReader objReader = objCommand.ExecuteReader())
                        {
                            while (objReader.Read())
                            {
                                CreatedObject.Data objData = new CreatedObject.Data();
                                objData.tag = objReader.GetString(objReader.GetOrdinal("type"));
                                objData.position = StringToVector3(objReader.GetString(objReader.GetOrdinal("position")));
                                objData.rotation = StringToQuaternion(objReader.GetString(objReader.GetOrdinal("rotation")));
                                objData.scale = StringToVector3(objReader.GetString(objReader.GetOrdinal("scale")));
                                level.createdObjects.Add(objData);
                            }
                        }
                    }

                    // Load floors
                    using (SqlCommand floorCommand = new SqlCommand("SELECT * FROM Floor WHERE level = @levelname", dbconn))
                    {
                        floorCommand.Parameters.AddWithValue("@levelname", name);
                        using (SqlDataReader floorReader = floorCommand.ExecuteReader())
                        {
                            while (floorReader.Read())
                            {
                                FloorData.Data floorData = new FloorData.Data();
                                floorData.floorNumber = floorReader.GetInt32(floorReader.GetOrdinal("floorNumber"));
                                if (!floorReader.IsDBNull(floorReader.GetOrdinal("floorplan")))
                                {
                                    floorData.floorPlanPath = floorReader.GetString(floorReader.GetOrdinal("floorplan"));
                                }
                                level.floors.Add(floorData);
                            }
                        }
                    }
                    Debug.Log("Level " + name + " loaded from database.");
                }
                else
                {
                    Debug.Log("Level " + name + " does not exist in database.");
                }
            }
        }

        dbconn.Close();

        return level;
    }

    private Vector3 StringToVector3(string s)
    {
        string[] split = s.Trim(new char[] { '(', ')' }).Split(',');
        return new Vector3(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture));
    }


    private Quaternion StringToQuaternion(string s)
    {
        string[] split = s.Trim(new char[] { '(', ')' }).Split(',');
        return new Quaternion(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture), float.Parse(split[3], CultureInfo.InvariantCulture));
    }

    public void DeleteLevel(string name, SqlConnection dbconn)
    {
        using (SqlCommand command = new SqlCommand("DELETE FROM Object WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Floor WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        Debug.Log("Level deleted");
    }

    public void DeleteLevel(string name)
    {
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("DELETE FROM Object WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Floor WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        Debug.Log("Level deleted");
        dbconn.Close();
    }

    public List<string> GetLevels()
    {
        List<string> levels = new List<string>();

        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=3D_Modular_Gaming_Tool;";

        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Level", dbconn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    levels.Add(reader.GetString(reader.GetOrdinal("levelname")));
                }
            }
        }

        dbconn.Close();

        Debug.Log("Levels: " + levels.Count);
        
        return levels;
    }

    public void addTimers(int maximumTime, int minimumTime, string levelName)
    {
        Debug.Log("Adding time to database...");
        Debug.Log("Max time: " + maximumTime + ", Min time: " + minimumTime + ", Level: " + levelName);

        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=3D_Modular_Gaming_Tool;";

        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("UPDATE level SET maximumTime = @maximumTime, minimumTime = @minimumTime WHERE levelName = @levelName", dbconn))
        {
            command.Parameters.AddWithValue("@maximumTime", maximumTime);
            command.Parameters.AddWithValue("@minimumTime", minimumTime);
            command.Parameters.AddWithValue("@levelName", levelName);
            command.ExecuteNonQuery();
        }

        dbconn.Close();
    }

    public void SetScore(float score)
    {
        string firstName = PlayerPrefs.GetString("PlayerFirstName");
        string lastName = PlayerPrefs.GetString("PlayerLastName");
        int playerId = -1;

        // Look up the player ID based on the first and last name
        using (SqlConnection dbconn = new SqlConnection(conn))
        {
            dbconn.Open();

            using (SqlCommand command = new SqlCommand("SELECT playerID FROM Player WHERE firstName = @firstName AND lastName = @lastName", dbconn))
            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        playerId = reader.GetInt32(0);
                    }
                }
            }

            dbconn.Close();
        }

        if (playerId != -1)
        {
            string levelName = PlayerPrefs.GetString("ActiveLevel");

            using (SqlConnection dbconn = new SqlConnection(conn))
            {
                dbconn.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO Score (score, level, playerId) VALUES (@score, @level, @playerId)", dbconn))
                {
                    command.Parameters.AddWithValue("@score", score);
                    command.Parameters.AddWithValue("@level", levelName);
                    command.Parameters.AddWithValue("@playerId", playerId);
                    command.ExecuteNonQuery();
                }

                dbconn.Close();
            }
        }
    }


}
