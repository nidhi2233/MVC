using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_PRACTICAL.Models;
using Npgsql;

namespace MVC_PRACTICAL.BAL
{
    public class TopicHelper
    {
        public readonly NpgsqlConnection _conn;

        public TopicHelper(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public List<topic> GetTopics()
        {
            try
            {
                var topicList = new List<topic>();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT c_topicid,c_topicname FROM t_assignedtask", _conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topicList.Add(new topic
                            {
                                TopicId = Convert.ToInt32(reader["c_topicid"].ToString()),
                                TopicName = reader["c_topicname"].ToString()
                            });
                        }
                    }
                }

                return topicList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public topic GetSelectedTopics(int id)
        {
            try
            {
                var topic = new topic();
                _conn.Open();
                using (var command = new NpgsqlCommand(@"SELECT c_topicid,c_topicname FROM t_assignedtask WHERE c_topicid = @c_topicid", _conn))
                {
                    command.Parameters.AddWithValue("c_topicid", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                                topic.TopicId = Convert.ToInt32(reader["c_topicid"].ToString());
                                topic.TopicName = reader["c_topicname"].ToString();
                        }
                    }
                }

                return topic;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool AddTopics(topic topic)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"INSERT INTO t_assingedtask (c_topicname) VALUES (@c_topicname)", _conn))
                {
                    command.Parameters.AddWithValue("c_topicname", topic.TopicName!);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool UpdateTopics(topic topic)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"UPDATE t_assingedtask SET c_topicname=@c_topicname WHERE c_topicid = @c_topicid", _conn))
                {
                    command.Parameters.AddWithValue("c_topicname", topic.TopicName!);
                    command.Parameters.AddWithValue("c_topicid", topic.TopicId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }

        public bool DeleteTopics(int id)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand(@"DELETE FROM t_assingedtask WHERE c_topicid = @c_topicid", _conn))
                {
                    command.Parameters.AddWithValue("c_topicid", id);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}