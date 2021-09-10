﻿using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RedisService
    {
        static CSRedisClient redisManger = null;

        static CSRedisClient GetClient()
        {
            return redisManger;
        }

        static RedisService()
        {
            redisManger = new CSRedisClient("127.0.0.1:6379,password=123456");
        }

        /// <summary>
        /// 设置hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetHash(string key, string field, string value)
        {
            try
            {
                GetClient().HSet(key, field, value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async static Task<bool> SetHashAsync(string key, string field, string value)
        {
            try
            {
                await GetClient().HSetAsync(key, field, value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetHashAndReplace(string key, string field, string value)
        {
            try
            {
                var client = GetClient();

                var ret = client.HGetAll(key);

                if (ret != null && ret.Any())
                {
                    foreach (var r in ret)
                    {
                        client.HDel(key, r.Key);
                    }
                }

                client.HSet(key, field, value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async static Task<bool> SetHashAndReplaceAsync(string key, string field, string value)
        {
            try
            {
                var client = GetClient();

                var ret = await client.HGetAllAsync(key);

                if (ret != null && ret.Any())
                {
                    foreach (var r in ret)
                    {
                        client.HDel(key, r.Key);
                    }
                }

                await client.HSetAsync(key, field, value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据表名，键名，获取hash值
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="field">键名</param>
        /// <returns></returns>
        public static string GetHash(string key, string field)
        {
            string result = "";
            try
            {

                result = GetClient().HGet(key, field);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public async static Task<string> GetHashAsync(string key, string field)
        {
            string result = "";
            try
            {

                result = await GetClient().HGetAsync(key, field);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        /// <summary>
        /// 获取指定key中所有字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetHashAll(string key)
        {
            try
            {

                var result = GetClient().HGetAll(key);
                return result;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 根据表名，键名，删除hash值
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="field">键名</param>
        /// <returns></returns>
        public static long DeleteHash(string key, string field)
        {
            long result = 0;
            try
            {
                result = GetClient().HDel(key, field);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public static bool HExists(string key, string field)
        {
            bool result = false;
            try
            {
                result = GetClient().HExists(key, field);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public static bool SetExpire(string key, int seconds)
        {
            bool result = false;
            try
            {
                result = GetClient().Expire(key, seconds);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public static bool DeleteAllKey(string key)
        {
            bool result = false;
            var client = GetClient();

            try
            {
                var allField = client.HGetAll(key);

                if (allField != null && allField.Any())
                {
                    foreach (var r in allField)
                    {
                        client.HDel(key, r.Key);
                    }
                }

                return true;
            }
            catch
            {
                return result;
            }
        }
    }
}
