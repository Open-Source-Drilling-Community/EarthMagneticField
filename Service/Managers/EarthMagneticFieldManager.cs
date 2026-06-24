using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using NORCE.Drilling.EarthMagneticField.Model;

namespace NORCE.Drilling.EarthMagneticField.Service.Managers
{
    /// <summary>
    /// A manager for EarthMagneticField. The manager implements the singleton pattern as defined by 
    /// Gamma, Erich, et al. "Design patterns: Abstraction and reuse of object-oriented design." 
    /// European Conference on Object-Oriented Programming. Springer, Berlin, Heidelberg, 1993.
    /// </summary>
    public class EarthMagneticFieldManager
    {
        private static EarthMagneticFieldManager? _instance = null;
        private readonly ILogger<EarthMagneticFieldManager> _logger;
        private readonly SqlConnectionManager _connectionManager;

        private EarthMagneticFieldManager(ILogger<EarthMagneticFieldManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public static EarthMagneticFieldManager GetInstance(ILogger<EarthMagneticFieldManager> logger, SqlConnectionManager connectionManager)
        {
            _instance ??= new EarthMagneticFieldManager(logger, connectionManager);
            return _instance;
        }

        public int Count
        {
            get
            {
                int count = 0;
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT COUNT(*) FROM EarthMagneticFieldTable";
                    try
                    {
                        using SqliteDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            count = (int)reader.GetInt64(0);
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to count records in the EarthMagneticFieldTable");
                    }
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
                return count;
            }
        }

        public bool Clear()
        {
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                bool success = false;
                using var transaction = connection.BeginTransaction();
                try
                {
                    //empty EarthMagneticFieldTable
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM EarthMagneticFieldTable";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    success = true;
                }
                catch (SqliteException ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Impossible to clear the EarthMagneticFieldTable");
                }
                return success;
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
                return false;
            }
        }

        public bool Contains(Guid guid)
        {
            int count = 0;
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM EarthMagneticFieldTable WHERE ID = ' {guid}'";
                try
                {
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        count = (int)reader.GetInt64(0);
                    }
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to count rows from EarthMagneticFieldTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return count >= 1;
        }

        /// <summary>
        /// Returns the list of Guid of all EarthMagneticField present in the microservice database 
        /// </summary>
        /// <returns>the list of Guid of all EarthMagneticField present in the microservice database</returns>
        public List<Guid>? GetAllEarthMagneticFieldId()
        {
            List<Guid> ids = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID FROM EarthMagneticFieldTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        Guid id = reader.GetGuid(0);
                        ids.Add(id);
                    }
                    _logger.LogInformation("Returning the list of ID of existing records from EarthMagneticFieldTable");
                    return ids;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from EarthMagneticFieldTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of MetaInfo of all EarthMagneticField present in the microservice database 
        /// </summary>
        /// <returns>the list of MetaInfo of all EarthMagneticField present in the microservice database</returns>
        public List<MetaInfo?>? GetAllEarthMagneticFieldMetaInfo()
        {
            List<MetaInfo?> metaInfos = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo FROM EarthMagneticFieldTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string mInfo = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(mInfo, JsonSettings.Options);
                        metaInfos.Add(metaInfo);
                    }
                    _logger.LogInformation("Returning the list of MetaInfo of existing records from EarthMagneticFieldTable");
                    return metaInfos;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from EarthMagneticFieldTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns a EarthMagneticField identified by its Guid from the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the EarthMagneticField retrieved from the database</returns>
        public Model.EarthMagneticField? GetEarthMagneticFieldById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    Model.EarthMagneticField? earthMagneticField = null;
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT EarthMagneticField FROM EarthMagneticFieldTable WHERE ID = '{guid}'";
                    try
                    {
                        using var reader = command.ExecuteReader();
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string data = reader.GetString(0);
                            earthMagneticField = JsonSerializer.Deserialize<Model.EarthMagneticField>(data, JsonSettings.Options);
                            if (earthMagneticField != null && earthMagneticField.MetaInfo != null && !earthMagneticField.MetaInfo.ID.Equals(guid))
                                throw new SqliteException("SQLite database corrupted: retrieved EarthMagneticField is null or has been jsonified with the wrong ID.", 1);
                        }
                        else
                        {
                            _logger.LogInformation("No EarthMagneticField of given ID in the database");
                            return null;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to get the EarthMagneticField with the given ID from EarthMagneticFieldTable");
                        return null;
                    }

                    // Finalizing
                    _logger.LogInformation("Returning the EarthMagneticField of given ID from EarthMagneticFieldTable");
                    return earthMagneticField;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticField ID is null or empty");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all EarthMagneticField present in the microservice database 
        /// </summary>
        /// <returns>the list of all EarthMagneticField present in the microservice database</returns>
        public List<Model.EarthMagneticField?>? GetAllEarthMagneticField()
        {
            List<Model.EarthMagneticField?> vals = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EarthMagneticField FROM EarthMagneticFieldTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string data = reader.GetString(0);
                        Model.EarthMagneticField? earthMagneticField = JsonSerializer.Deserialize<Model.EarthMagneticField>(data, JsonSettings.Options);
                        vals.Add(earthMagneticField);
                    }
                    _logger.LogInformation("Returning the list of existing EarthMagneticField from EarthMagneticFieldTable");
                    return vals;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get EarthMagneticField from EarthMagneticFieldTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all EarthMagneticField present with given type in the microservice database 
        /// </summary>
        /// <returns>the list of all EarthMagneticField present with given type in the microservice database</returns>
        public List<Model.EarthMagneticField?>? GetAllCompletedEarthMagneticField(bool isCompleted)
        {
            List<Model.EarthMagneticField?> vals = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                string type = isCompleted ? "Completed" : "Raw";
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT EarthMagneticField FROM EarthMagneticFieldTable WHERE Type = '{type}'";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string data = reader.GetString(0);
                        Model.EarthMagneticField? earthMagneticField = JsonSerializer.Deserialize<Model.EarthMagneticField>(data, JsonSettings.Options);
                        vals.Add(earthMagneticField);
                    }
                    _logger.LogInformation("Returning the list of existing EarthMagneticField from EarthMagneticFieldTable");
                    return vals;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get EarthMagneticField from EarthMagneticFieldTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }
        /// <summary>
        /// Adds the given EarthMagneticField to the microservice database
        /// </summary>
        /// <param name="earthMagneticField"></param>
        /// <returns>true if the given EarthMagneticField has been added successfully</returns>
        public bool AddEarthMagneticField(Model.EarthMagneticField? earthMagneticField)
        {
            if (earthMagneticField != null && earthMagneticField.MetaInfo != null && earthMagneticField.MetaInfo.ID != Guid.Empty)
            {
                //update EarthMagneticFieldTable
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using SqliteTransaction transaction = connection.BeginTransaction();
                    bool success = true;
                    try
                    {
                        //add the EarthMagneticField to the EarthMagneticFieldTable
                        string metaInfo = JsonSerializer.Serialize(earthMagneticField.MetaInfo, JsonSettings.Options);
                        string data = JsonSerializer.Serialize(earthMagneticField, JsonSettings.Options);
                        string type = (earthMagneticField.Type == EarthMagneticFieldType.Raw) ? "Raw" : "Completed"; 
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO EarthMagneticFieldTable (" +
                            "ID, " +
                            "MetaInfo, " +
                            "Type, " +
                            "EarthMagneticField" +
                            ") VALUES (" +
                            $"'{earthMagneticField.MetaInfo.ID}', " +
                            $"'{metaInfo}', " +
                            $"'{type}', " +
                            $"'{data}'" +
                            ")";
                        int count = command.ExecuteNonQuery();
                        if (count != 1)
                        {
                            _logger.LogWarning("Impossible to insert the given EarthMagneticField into the EarthMagneticFieldTable");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to add the given EarthMagneticField into EarthMagneticFieldTable");
                        success = false;
                    }
                    //finalizing
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Added the given EarthMagneticField of given ID into the EarthMagneticFieldTable successfully");
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return success;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The EarthMagneticField ID or the ID of its input are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Performs calculation on the given EarthMagneticField and updates it in the microservice database
        /// </summary>
        /// <param name="earthMagneticField"></param>
        /// <returns>true if the given EarthMagneticField has been updated successfully</returns>
        public bool UpdateEarthMagneticFieldById(Guid guid, Model.EarthMagneticField? earthMagneticField)
        {
            bool success = true;
            if (guid != Guid.Empty && earthMagneticField != null && earthMagneticField.MetaInfo != null && earthMagneticField.MetaInfo.ID == guid)
            {
                //update EarthMagneticFieldTable
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using SqliteTransaction transaction = connection.BeginTransaction();
                    //update fields in EarthMagneticFieldTable
                    try
                    {
                        string metaInfo = JsonSerializer.Serialize(earthMagneticField.MetaInfo, JsonSettings.Options);
                        earthMagneticField.LastModificationDate = DateTimeOffset.UtcNow;
                        string data = JsonSerializer.Serialize(earthMagneticField, JsonSettings.Options);
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE EarthMagneticFieldTable SET " +
                            $"MetaInfo = '{metaInfo}', " +
                            $"EarthMagneticField = '{data}' " +
                            $"WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count != 1)
                        {
                            _logger.LogWarning("Impossible to update the EarthMagneticField");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to update the EarthMagneticField");
                        success = false;
                    }

                    // Finalizing
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Updated the given EarthMagneticField successfully");
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The EarthMagneticField ID or the ID of some of its attributes are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Deletes the EarthMagneticField of given ID from the microservice database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the EarthMagneticField was deleted from the microservice database</returns>
        public bool DeleteEarthMagneticFieldById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();
                    bool success = true;
                    //delete EarthMagneticField from EarthMagneticFieldTable
                    try
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"DELETE FROM EarthMagneticFieldTable WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count < 0)
                        {
                            _logger.LogWarning("Impossible to delete the EarthMagneticField of given ID from the EarthMagneticFieldTable");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to delete the EarthMagneticField of given ID from EarthMagneticFieldTable");
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Removed the EarthMagneticField of given ID from the EarthMagneticFieldTable successfully");
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return success;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The EarthMagneticField ID is null or empty");
            }
            return false;
        }
    }
}