using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OSDC.DotnetLibraries.General.DataManagement;
using Microsoft.Data.Sqlite;
using System.Text.Json;
using NORCE.Drilling.EarthMagneticField.Model;

namespace NORCE.Drilling.EarthMagneticField.Service.Managers
{

    /// <summary>
    /// A manager for EarthMagneticFieldCalculationOrder. The manager implements the singleton pattern as defined by 
    /// Gamma, Erich, et al. "Design patterns: Abstraction and reuse of object-oriented design." 
    /// European Conference on Object-Oriented Programming. Springer, Berlin, Heidelberg, 1993.
    /// </summary>
    public class EarthMagneticFieldCalculationOrderManager
    {
        private static EarthMagneticFieldCalculationOrderManager? _instance = null;
        private readonly ILogger<EarthMagneticFieldCalculationOrderManager> _logger;
        private readonly SqlConnectionManager _connectionManager;

        private EarthMagneticFieldCalculationOrderManager(ILogger<EarthMagneticFieldCalculationOrderManager> logger, SqlConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public static EarthMagneticFieldCalculationOrderManager GetInstance(ILogger<EarthMagneticFieldCalculationOrderManager> logger, SqlConnectionManager connectionManager)
        {
            _instance ??= new EarthMagneticFieldCalculationOrderManager(logger, connectionManager);
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
                    command.CommandText = "SELECT COUNT(*) FROM EarthMagneticFieldCalculationOrderTable";
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
                        _logger.LogError(ex, "Impossible to count records in the EarthMagneticFieldCalculationOrderTable");
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
                    //empty EarthMagneticFieldCalculationOrderTable
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM EarthMagneticFieldCalculationOrderTable";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    success = true;
                }
                catch (SqliteException ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Impossible to clear the EarthMagneticFieldCalculationOrderTable");
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
                command.CommandText = $"SELECT COUNT(*) FROM EarthMagneticFieldCalculationOrderTable WHERE ID = '{guid}'";
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
                    _logger.LogError(ex, "Impossible to count rows from EarthMagneticFieldCalculationOrderTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return count >= 1;
        }
        private static Model.EarthMagneticFieldCalculationOrderLight CreateDataLightInstance(Model.EarthMagneticFieldCalculationOrder earthMagneticFieldCalculationOrder)
        {
            return new Model.EarthMagneticFieldCalculationOrderLight()
                {
                    MetaInfo = earthMagneticFieldCalculationOrder.MetaInfo,
                    Name = earthMagneticFieldCalculationOrder.Name,
                    Description = earthMagneticFieldCalculationOrder.Description,
                    CreationDate = earthMagneticFieldCalculationOrder.CreationDate,
                    LastModificationDate = earthMagneticFieldCalculationOrder.LastModificationDate
                };
        }
        /// <summary>
        /// Returns the list of Guid of all EarthMagneticFieldCalculationOrder present in the microservice database 
        /// </summary>
        /// <returns>the list of Guid of all EarthMagneticFieldCalculationOrder present in the microservice database</returns>
        public List<Guid>? GetAllEarthMagneticFieldCalculationOrderId()
        {
            List<Guid> ids = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ID FROM EarthMagneticFieldCalculationOrderTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        Guid id = reader.GetGuid(0);
                        ids.Add(id);
                    }
                    _logger.LogInformation("Returning the list of ID of existing records from EarthMagneticFieldCalculationOrderTable");
                    return ids;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from EarthMagneticFieldCalculationOrderTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of MetaInfo of all EarthMagneticFieldCalculationOrder present in the microservice database 
        /// </summary>
        /// <returns>the list of MetaInfo of all EarthMagneticFieldCalculationOrder present in the microservice database</returns>
        public List<MetaInfo?>? GetAllEarthMagneticFieldCalculationOrderMetaInfo()
        {
            List<MetaInfo?> metaInfos = new();
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo FROM EarthMagneticFieldCalculationOrderTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string mInfo = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(mInfo, JsonSettings.Options);
                        metaInfos.Add(metaInfo);
                    }
                    _logger.LogInformation("Returning the list of MetaInfo of existing records from EarthMagneticFieldCalculationOrderTable");
                    return metaInfos;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get IDs from EarthMagneticFieldCalculationOrderTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the EarthMagneticFieldCalculationOrder identified by its Guid from the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the EarthMagneticFieldCalculationOrder identified by its Guid from the microservice database</returns>
        public Model.EarthMagneticFieldCalculationOrder? GetEarthMagneticFieldCalculationOrderById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    Model.EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder;
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT EarthMagneticFieldCalculationOrder FROM EarthMagneticFieldCalculationOrderTable WHERE ID = '{guid}'";
                    try
                    {
                        using var reader = command.ExecuteReader();
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string data = reader.GetString(0);
                            earthMagneticFieldCalculationOrder = JsonSerializer.Deserialize<Model.EarthMagneticFieldCalculationOrder>(data, JsonSettings.Options);
                            if (earthMagneticFieldCalculationOrder != null && earthMagneticFieldCalculationOrder.MetaInfo != null && !earthMagneticFieldCalculationOrder.MetaInfo.ID.Equals(guid))
                                throw new SqliteException("SQLite database corrupted: returned EarthMagneticFieldCalculationOrder is null or has been jsonified with the wrong ID.", 1);
                        }
                        else
                        {
                            _logger.LogInformation("No EarthMagneticFieldCalculationOrder of given ID in the database");
                            return null;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to get the EarthMagneticFieldCalculationOrder with the given ID from EarthMagneticFieldCalculationOrderTable");
                        return null;
                    }
                    _logger.LogInformation("Returning the EarthMagneticFieldCalculationOrder of given ID from EarthMagneticFieldCalculationOrderTable");
                    return earthMagneticFieldCalculationOrder;
                }
                else
                {
                    _logger.LogWarning("Impossible to access the SQLite database");
                }
            }
            else
            {
                _logger.LogWarning("The given EarthMagneticFieldCalculationOrder ID is null or empty");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all EarthMagneticFieldCalculationOrder present in the microservice database 
        /// </summary>
        /// <returns>the list of all EarthMagneticFieldCalculationOrder present in the microservice database</returns>
        public List<Model.EarthMagneticFieldCalculationOrder?>? GetAllEarthMagneticFieldCalculationOrder()
        {
            List<Model.EarthMagneticFieldCalculationOrder?> vals = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EarthMagneticFieldCalculationOrder FROM EarthMagneticFieldCalculationOrderTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string data = reader.GetString(0);
                        Model.EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder = JsonSerializer.Deserialize<Model.EarthMagneticFieldCalculationOrder>(data, JsonSettings.Options);
                        vals.Add(earthMagneticFieldCalculationOrder);
                    }
                    _logger.LogInformation("Returning the list of existing EarthMagneticFieldCalculationOrder from EarthMagneticFieldCalculationOrderTable");
                    return vals;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get EarthMagneticFieldCalculationOrder from EarthMagneticFieldCalculationOrderTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

        /// <summary>
        /// Returns the list of all EarthMagneticFieldCalculationOrderLight present in the microservice database 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>the list of EarthMagneticFieldCalculationOrderLight present in the microservice database</returns>
        public List<Model.EarthMagneticFieldCalculationOrderLight>? GetAllEarthMagneticFieldCalculationOrderLight()
        {
            List<Model.EarthMagneticFieldCalculationOrderLight>? earthMagneticFieldCalculationOrderLightList = [];
            var connection = _connectionManager.GetConnection();
            if (connection != null)
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MetaInfo, EarthMagneticFieldCalculationOrderLight FROM EarthMagneticFieldCalculationOrderTable";
                try
                {
                    using var reader = command.ExecuteReader();
                    while (reader.Read() && !reader.IsDBNull(0))
                    {
                        string metaInfoStr = reader.GetString(0);
                        MetaInfo? metaInfo = JsonSerializer.Deserialize<MetaInfo>(metaInfoStr, JsonSettings.Options);
                        Model.EarthMagneticFieldCalculationOrderLight? earthMagneticFieldCalculationOrderLight = JsonSerializer.Deserialize<Model.EarthMagneticFieldCalculationOrderLight>(reader.GetString(1), JsonSettings.Options);
                        if (earthMagneticFieldCalculationOrderLight != null)
                        {
                            earthMagneticFieldCalculationOrderLightList.Add(earthMagneticFieldCalculationOrderLight);                            
                        }
                    }
                    _logger.LogInformation("Returning the list of existing EarthMagneticFieldCalculationOrderLight from EarthMagneticFieldCalculationOrderTable");
                    return earthMagneticFieldCalculationOrderLightList;
                }
                catch (SqliteException ex)
                {
                    _logger.LogError(ex, "Impossible to get light datas from EarthMagneticFieldCalculationOrderTable");
                }
            }
            else
            {
                _logger.LogWarning("Impossible to access the SQLite database");
            }
            return null;
        }

         private bool AddCompletedEarthMagneticField(
            SqliteConnection connection,
            EarthMagneticFieldCalculationOrder magneticFieldCalculationOrder)
        {
            bool success = false;
            try
            {
                if (magneticFieldCalculationOrder!.CompletedEarthMagneticFieldTable == null)
                {
                    success = false;
                    _logger.LogWarning("Impossible to insert the given EarthMagneticField into the EarthMagneticFieldTable");                             
                }
                else
                {  
                    Model.EarthMagneticField magneticField = magneticFieldCalculationOrder.CompletedEarthMagneticFieldTable;
                    //add the EarthMagneticField to the EarthMagneticFieldTable
                    string metaInfo = JsonSerializer.Serialize(magneticField.MetaInfo, JsonSettings.Options);
                    string data = JsonSerializer.Serialize(magneticField, JsonSettings.Options);
                    string type = magneticField.Type == EarthMagneticFieldType.Raw ? "Raw" : "Completed";
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO EarthMagneticFieldTable (" +
                        "ID, " +
                        "MetaInfo, " +
                        "Type," +
                        "EarthMagneticField" +
                        ") VALUES (" +
                        $"'{magneticField.MetaInfo!.ID}', " +
                        $"'{metaInfo}', " +
                        $"'{type}', " +
                        $"'{data}'" +
                        ")";
                    int count = command.ExecuteNonQuery();
                    if (count != 1)
                    {
                        _logger.LogWarning("Impossible to insert the given EarthMagneticField into the EarthMagneticTable");
                        success = false;
                    }    
                }                                                                                  
            }
            catch (SqliteException ex)
            {
                _logger.LogError(ex, "Impossible to add the given completed Earth Magnetic Field  into EarthMagneticFieldTable");
                success = false;                            
            }
            
            return success;
        }
        /// <summary>
        /// Performs calculation on the given EarthMagneticFieldCalculationOrder and adds it to the microservice database
        /// </summary>
        /// <param name="earthMagneticFieldCalculationOrder"></param>
        /// <returns>true if the given EarthMagneticFieldCalculationOrder has been added successfully to the microservice database</returns>
        public bool AddEarthMagneticFieldCalculationOrder(Model.EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder)
        {
            if (earthMagneticFieldCalculationOrder != null && earthMagneticFieldCalculationOrder.MetaInfo != null && earthMagneticFieldCalculationOrder.MetaInfo.ID != Guid.Empty)
            {
                //calculate outputs
                if (!earthMagneticFieldCalculationOrder.Calculate())
                {
                    _logger.LogWarning("Impossible to calculate outputs for the given EarthMagneticFieldCalculationOrder");
                    return false;
                }

                //if successful, check if another parent data with the same ID was calculated/added during the calculation time
                Model.EarthMagneticFieldCalculationOrder? newEarthMagneticFieldCalculationOrder = GetEarthMagneticFieldCalculationOrderById(earthMagneticFieldCalculationOrder.MetaInfo.ID);
                if (newEarthMagneticFieldCalculationOrder == null)
                {
                    //update EarthMagneticFieldCalculationOrderTable
                    var connection = _connectionManager.GetConnection();
                    if (connection != null)
                    {
                        using SqliteTransaction transaction = connection.BeginTransaction();
                        bool success = true;
                        try
                        {
                            //add the EarthMagneticFieldCalculationOrder to the EarthMagneticFieldCalculationOrderTable
                            string metaInfo = JsonSerializer.Serialize(earthMagneticFieldCalculationOrder.MetaInfo, JsonSettings.Options);
                      
                            Model.EarthMagneticFieldCalculationOrderLight earthMagneticFieldCalculationOrderLight = CreateDataLightInstance(earthMagneticFieldCalculationOrder);
                            string dataLight = JsonSerializer.Serialize(earthMagneticFieldCalculationOrderLight, JsonSettings.Options);                           

                            string? cDate = null;
                            if (earthMagneticFieldCalculationOrder.CreationDate != null)
                                cDate = ((DateTimeOffset)earthMagneticFieldCalculationOrder.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            string? lDate = null;
                            if (earthMagneticFieldCalculationOrder.LastModificationDate != null)
                                lDate = ((DateTimeOffset)earthMagneticFieldCalculationOrder.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                            string data = JsonSerializer.Serialize(earthMagneticFieldCalculationOrder, JsonSettings.Options);
                            
                            var command = connection.CreateCommand();
                            command.CommandText = "INSERT INTO EarthMagneticFieldCalculationOrderTable (" +
                                "ID, " +
                                "MetaInfo, " +
                                "EarthMagneticFieldCalculationOrderLight, " +                                
                                "CreationDate, " +
                                "LastModificationDate, " +
                                "EarthMagneticFieldCalculationOrder" +
                                ") VALUES (" +
                                $"'{earthMagneticFieldCalculationOrder.MetaInfo.ID}', " +
                                $"'{metaInfo}', " +
                                $"'{dataLight}', " +
                                $"'{cDate}', " +
                                $"'{lDate}', " +
                                $"'{data}'" +
                                ")";
                            int count = command.ExecuteNonQuery();
                            if (count != 1)
                            {
                                _logger.LogWarning("Impossible to insert the given EarthMagneticFieldCalculationOrder into the EarthMagneticFieldCalculationOrderTable");
                                success = false;
                            }
                            AddCompletedEarthMagneticField(connection, earthMagneticFieldCalculationOrder);
                        }
                        catch (SqliteException ex)
                        {
                            _logger.LogError(ex, "Impossible to add the given EarthMagneticFieldCalculationOrder into EarthMagneticFieldCalculationOrderTable");
                            success = false;
                        }
                        //finalizing SQL transaction
                        if (success)
                        {
                            transaction.Commit();
                            _logger.LogInformation("Added the given EarthMagneticFieldCalculationOrder of given ID into the EarthMagneticFieldCalculationOrderTable successfully");
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
                    _logger.LogWarning("Impossible to post EarthMagneticFieldCalculationOrder. ID already found in database.");
                    return false;
                }

            }
            else
            {
                _logger.LogWarning("The EarthMagneticFieldCalculationOrder ID or the ID of its input are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Performs calculation on the given EarthMagneticFieldCalculationOrder and updates it in the microservice database
        /// </summary>
        /// <param name="earthMagneticFieldCalculationOrder"></param>
        /// <returns>true if the given EarthMagneticFieldCalculationOrder has been updated successfully</returns>
        public bool UpdateEarthMagneticFieldCalculationOrderById(Guid guid, Model.EarthMagneticFieldCalculationOrder? earthMagneticFieldCalculationOrder)
        {
            bool success = true;
            if (guid != Guid.Empty && earthMagneticFieldCalculationOrder != null && earthMagneticFieldCalculationOrder.MetaInfo != null && earthMagneticFieldCalculationOrder.MetaInfo.ID == guid)
            {
                //calculate outputs
                if (!earthMagneticFieldCalculationOrder.Calculate())
                {
                    _logger.LogWarning("Impossible to calculate outputs of the given EarthMagneticFieldCalculationOrder");
                    return false;
                }
                //update EarthMagneticFieldCalculationOrderTable
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using SqliteTransaction transaction = connection.BeginTransaction();
                    //update fields in EarthMagneticFieldCalculationOrderTable
                    try
                    {
                        string metaInfo = JsonSerializer.Serialize(earthMagneticFieldCalculationOrder.MetaInfo, JsonSettings.Options);
                        Model.EarthMagneticFieldCalculationOrderLight earthMagneticFieldCalculationOrderLight = CreateDataLightInstance(earthMagneticFieldCalculationOrder);
                        string dataLight = JsonSerializer.Serialize(earthMagneticFieldCalculationOrderLight, JsonSettings.Options);                           
                        string? cDate = null;
                        if (earthMagneticFieldCalculationOrder.CreationDate != null)
                            cDate = ((DateTimeOffset)earthMagneticFieldCalculationOrder.CreationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        earthMagneticFieldCalculationOrder.LastModificationDate = DateTimeOffset.UtcNow;
                        string? lDate = ((DateTimeOffset)earthMagneticFieldCalculationOrder.LastModificationDate).ToString(SqlConnectionManager.DATE_TIME_FORMAT);
                        string data = JsonSerializer.Serialize(earthMagneticFieldCalculationOrder, JsonSettings.Options);
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE EarthMagneticFieldCalculationOrderTable SET " +
                            $"MetaInfo = '{metaInfo}', " +
                            $"EarthMagneticFieldCalculationOrderLight = '{dataLight}', " +                              
                            $"CreationDate = '{cDate}', " +
                            $"LastModificationDate = '{lDate}', " +
                            $"EarthMagneticFieldCalculationOrder = '{data}' " +
                            $"WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count != 1)
                        {
                            _logger.LogWarning("Impossible to update the EarthMagneticFieldCalculationOrder");
                            success = false;
                        }
                        AddCompletedEarthMagneticField(connection, earthMagneticFieldCalculationOrder);
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to update the EarthMagneticFieldCalculationOrder");
                        success = false;
                    }

                    // Finalizing
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Updated the given EarthMagneticFieldCalculationOrder successfully");
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
                _logger.LogWarning("The EarthMagneticFieldCalculationOrder ID or the ID of some of its attributes are null or empty");
            }
            return false;
        }

        /// <summary>
        /// Deletes the EarthMagneticFieldCalculationOrder of given ID from the microservice database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if the EarthMagneticFieldCalculationOrder was deleted from the microservice database</returns>
        public bool DeleteEarthMagneticFieldCalculationOrderById(Guid guid)
        {
            if (!guid.Equals(Guid.Empty))
            {
                var connection = _connectionManager.GetConnection();
                if (connection != null)
                {
                    using var transaction = connection.BeginTransaction();
                    bool success = true;
                    //delete EarthMagneticFieldCalculationOrder from EarthMagneticFieldCalculationOrderTable
                    try
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"DELETE FROM EarthMagneticFieldCalculationOrderTable WHERE ID = '{guid}'";
                        int count = command.ExecuteNonQuery();
                        if (count < 0)
                        {
                            _logger.LogWarning("Impossible to delete the EarthMagneticFieldCalculationOrder of given ID from the EarthMagneticFieldCalculationOrderTable");
                            success = false;
                        }
                    }
                    catch (SqliteException ex)
                    {
                        _logger.LogError(ex, "Impossible to delete the EarthMagneticFieldCalculationOrder of given ID from EarthMagneticFieldCalculationOrderTable");
                        success = false;
                    }
                    if (success)
                    {
                        transaction.Commit();
                        _logger.LogInformation("Removed the EarthMagneticFieldCalculationOrder of given ID from the EarthMagneticFieldCalculationOrderTable successfully");
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
                _logger.LogWarning("The EarthMagneticFieldCalculationOrder ID is null or empty");
            }
            return false;
        }
    }
}