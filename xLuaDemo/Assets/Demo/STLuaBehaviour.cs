/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace XLuaTest
{
    [System.Serializable]
    public class STInjection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class STLuaBehaviour : MonoBehaviour
    {
        public TextAsset luaScript;
        public STInjection[] injections;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaFixedUpdate;
        private Action luaOnDestroy;

        private Action luaOnCollisionEnter;
        private Action luaOnCollisionExit;

        private LuaTable scriptEnv;

        void Awake()
        {
            scriptEnv = luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }

            luaEnv.DoString(luaScript.text, "LuaShpere", scriptEnv);

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("fixedUpdate", out luaFixedUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            scriptEnv.Get("onCollisionEnter", out luaOnCollisionEnter);
            scriptEnv.Get("onCollisionExit", out luaOnCollisionExit);

            if (luaAwake != null)
            {
                luaAwake();
            }
        }

        // Use this for initialization
        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }

        private void FixedUpdate()
        {
            if (luaFixedUpdate != null)
            {
                luaFixedUpdate();
            }
        }

        void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;

            luaOnCollisionEnter = null;
            luaOnCollisionExit = null;

            scriptEnv.Dispose();
            injections = null;
        }

        // 碰撞开始
        void OnCollisionEnter(Collision collision)
        {
            if (luaOnCollisionEnter != null)
            {
                scriptEnv.Set("collision", collision.gameObject);
                luaOnCollisionEnter();
            }
        }

        // 碰撞结束
        void OnCollisionExit(Collision collision)
        {
            if (luaOnCollisionExit != null)
            {
                scriptEnv.Set("collision", collision.gameObject);
                luaOnCollisionExit();
            }
        }

        public void JumpClick()
        {
            Action luaJumpClick = scriptEnv.Get<Action>("onJumpClick");
            if (luaJumpClick != null)
            {
                luaJumpClick();
            }
        }
    }
}
