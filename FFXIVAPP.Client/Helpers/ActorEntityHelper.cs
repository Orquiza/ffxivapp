﻿// FFXIVAPP.Client
// ActorEntityHelper.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client.Helpers
{
    public static class ActorEntityHelper
    {
        public static ActorEntity ResolveActorFromBytes(byte[] source, bool isCurrentUser = false, ActorEntity entry = null)
        {
            entry = entry ?? new ActorEntity();
            try
            {
                uint targetID;
                uint pcTargetID;
                entry.MapIndex = 0;
                entry.TargetID = 0;
                switch (Settings.Default.GameLanguage)
                {
                    case "Chinese":
                        entry.Name = MemoryHandler.Instance.GetStringFromBytes(source, 48);
                        entry.ID = BitConverter.ToUInt32(source, 0x74);
                        entry.NPCID1 = BitConverter.ToUInt32(source, 0x78);
                        entry.NPCID2 = BitConverter.ToUInt32(source, 0x80);
                        entry.OwnerID = BitConverter.ToUInt32(source, 0x84);
                        entry.Type = (Actor.Type) source[0x8A];
                        entry.TargetType = (Actor.TargetType) source[0x8C];
                        entry.GatheringStatus = source[0x8E];
                        entry.Distance = source[0x8F];
                        entry.X = BitConverter.ToSingle(source, 0xA0);
                        entry.Z = BitConverter.ToSingle(source, 0xA4);
                        entry.Y = BitConverter.ToSingle(source, 0xA8);
                        entry.Heading = BitConverter.ToSingle(source, 0xB0);
                        entry.Fate = BitConverter.ToUInt32(source, 0xE4); // ??
                        entry.GatheringInvisible = source[0x11C]; // ??
                        entry.ModelID = BitConverter.ToUInt32(source, 0x174);
                        entry.ActionStatus = (Actor.ActionStatus) source[0x17C];
                        entry.IsGM = BitConverter.ToBoolean(source, 0x183); // ?
                        entry.Icon = (Actor.Icon) source[0x18C];
                        entry.Status = (Actor.Status) source[0x18E];
                        entry.ClaimedByID = BitConverter.ToUInt32(source, 0x1A0);
                        targetID = BitConverter.ToUInt32(source, 0x1A8);
                        pcTargetID = BitConverter.ToUInt32(source, 0xAA8);
                        entry.Job = (Actor.Job) source[0x17C0];
                        entry.Level = source[0x17C1];
                        entry.GrandCompany = source[0x17C3];
                        entry.GrandCompanyRank = source[0x17C4];
                        entry.Title = source[0x17C6];
                        entry.HPCurrent = BitConverter.ToInt32(source, 0x17C8);
                        entry.HPMax = BitConverter.ToInt32(source, 0x17CC);
                        entry.MPCurrent = BitConverter.ToInt32(source, 0x17D0);
                        entry.MPMax = BitConverter.ToInt32(source, 0x17D4);
                        entry.TPCurrent = BitConverter.ToInt16(source, 0x17D8);
                        entry.TPMax = 1000;
                        entry.GPCurrent = BitConverter.ToInt16(source, 0x17DA);
                        entry.GPMax = BitConverter.ToInt16(source, 0x17DC);
                        entry.CPCurrent = BitConverter.ToInt16(source, 0x17DE);
                        entry.CPMax = BitConverter.ToInt16(source, 0x17E0);
                        entry.Race = source[0x2E58]; // ??
                        entry.Sex = (Actor.Sex) source[0x2E59]; //?
                        entry.IsCasting = BitConverter.ToBoolean(source, 0x32E0) && BitConverter.ToBoolean(source, 0x32E1);
                        entry.CastingID = BitConverter.ToInt16(source, 0x32E4);
                        entry.CastingTargetID = BitConverter.ToUInt32(source, 0x32F0);
                        entry.CastingProgress = BitConverter.ToSingle(source, 0x3314);
                        entry.CastingTime = BitConverter.ToSingle(source, 0x33F8);
                        entry.Coordinate = new Coordinate(entry.X, entry.Z, entry.Y);
                        break;
                    default:
                        entry.Name = MemoryHandler.Instance.GetStringFromBytes(source, 48);
                        entry.ID = BitConverter.ToUInt32(source, 0x74);
                        entry.NPCID1 = BitConverter.ToUInt32(source, 0x7C);
                        entry.NPCID2 = BitConverter.ToUInt32(source, 0x80);
                        entry.OwnerID = BitConverter.ToUInt32(source, 0x84);
                        entry.Type = (Actor.Type) source[0x8A];
                        entry.TargetType = (Actor.TargetType) source[0x8C];
                        entry.GatheringStatus = source[0x8F];
                        entry.Distance = source[0x91];
                        var defaultBaseOffset = MemoryHandler.Instance.ProcessModel.IsWin64 ? 0x10 : 0;
                        entry.X = BitConverter.ToSingle(source, 0xA0 + defaultBaseOffset);
                        entry.Z = BitConverter.ToSingle(source, 0xA4 + defaultBaseOffset);
                        entry.Y = BitConverter.ToSingle(source, 0xA8 + defaultBaseOffset);
                        entry.Heading = BitConverter.ToSingle(source, 0xB0 + defaultBaseOffset);
                        entry.HitBoxRadius = BitConverter.ToSingle(source, 0xC0 + defaultBaseOffset);
                        entry.Fate = BitConverter.ToUInt32(source, 0xE4 + defaultBaseOffset); // ??
                        entry.GatheringInvisible = source[0x11C]; // ??
                        entry.ModelID = BitConverter.ToUInt32(source, 0x174);
                        entry.ActionStatus = (Actor.ActionStatus)source[0x16C];
                        // 0x17D - 0 = Green name, 4 = non-agro (yellow name)
                        entry.IsGM = BitConverter.ToBoolean(source, 0x183); // ?
                        entry.Icon = (Actor.Icon) source[0x18C];
                        entry.Status = (Actor.Status) source[0x17E];
                        entry.ClaimedByID = BitConverter.ToUInt32(source, 0x180);
                        targetID = BitConverter.ToUInt32(source, 0x818);
                        pcTargetID = targetID; //BitConverter.ToUInt32(source, 0x938); // no longer exists?
                        var defaultStatOffset = MemoryHandler.Instance.ProcessModel.IsWin64 ? 0x230 : 0;
                        entry.Job = (Actor.Job) source[0x1230 + defaultStatOffset];
                        entry.Level = source[0x1231 + defaultStatOffset];
                        entry.GrandCompany = source[0x1233 + defaultStatOffset];
                        entry.GrandCompanyRank = source[0x1234 + defaultStatOffset];
                        entry.Title = source[0x12B6 + defaultStatOffset];
                        entry.HPCurrent = BitConverter.ToInt32(source, 0x1238 + defaultStatOffset);
                        entry.HPMax = BitConverter.ToInt32(source, 0x123C + defaultStatOffset);
                        entry.MPCurrent = BitConverter.ToInt32(source, 0x1240 + defaultStatOffset);
                        entry.MPMax = BitConverter.ToInt32(source, 0x1244 + defaultStatOffset);
                        entry.TPCurrent = BitConverter.ToInt16(source, 0x1248 + defaultStatOffset);
                        entry.TPMax = 1000;
                        entry.GPCurrent = BitConverter.ToInt16(source, 0x124A + defaultStatOffset);
                        entry.GPMax = BitConverter.ToInt16(source, 0x124C + defaultStatOffset);
                        entry.CPCurrent = BitConverter.ToInt16(source, 0x124E + defaultStatOffset);
                        entry.CPMax = BitConverter.ToInt16(source, 0x1250 + defaultStatOffset);
                        //entry.Race = source[0x2578]; // ??
                        //entry.Sex = (Actor.Sex) source[0x2579]; //?
                        entry.IsCasting = BitConverter.ToBoolean(source, 0x1690) && BitConverter.ToBoolean(source, 0x1691); // 0x2C90);
                        entry.CastingID = BitConverter.ToInt16(source, 0x1694); // 0x2C94);
                        entry.CastingTargetID = BitConverter.ToUInt32(source, 0x16A0); // 0x2CA0);
                        entry.CastingProgress = BitConverter.ToSingle(source, 0x16C4); // 0x2CC4);
                        entry.CastingTime = BitConverter.ToSingle(source, 0x16C8); // 0x2DA8);
                        entry.Coordinate = new Coordinate(entry.X, entry.Z, entry.Y);
                        break;
                }
                if (targetID > 0)
                {
                    entry.TargetID = (int) targetID;
                }
                else
                {
                    if (pcTargetID > 0)
                    {
                        entry.TargetID = (int) pcTargetID;
                    }
                }
                if (entry.CastingTargetID == 3758096384)
                {
                    entry.CastingTargetID = 0;
                }
                entry.MapIndex = 0;
                var limit = 60;
                switch (entry.Type)
                {
                    case Actor.Type.PC:
                        limit = 30;
                        break;
                }
                entry.StatusEntries = new List<StatusEntry>();
                const int statusSize = 12;
                var statusesSource = new byte[limit * statusSize];
                switch (Settings.Default.GameLanguage)
                {
                    case "Chinese":
                        Buffer.BlockCopy(source, 0x3168, statusesSource, 0, limit * statusSize);
                        break;
                    default:
                        var defaultStatusEffectOffset = MemoryHandler.Instance.ProcessModel.IsWin64 ? 0x3740 : 0x1518;
                        Buffer.BlockCopy(source, defaultStatusEffectOffset, statusesSource, 0, limit * statusSize);
                        break;
                }
                for (var i = 0; i < limit; i++)
                {
                    var statusSource = new byte[statusSize];
                    Buffer.BlockCopy(statusesSource, i * statusSize, statusSource, 0, statusSize);
                    var statusEntry = new StatusEntry
                    {
                        TargetEntity = entry,
                        TargetName = entry.Name,
                        StatusID = BitConverter.ToInt16(statusSource, 0x0),
                        Stacks = statusSource[0x2],
                        Duration = BitConverter.ToSingle(statusSource, 0x4),
                        CasterID = BitConverter.ToUInt32(statusSource, 0x8)
                    };
                    try
                    {
                        var pc = PCWorkerDelegate.GetNPCEntity(statusEntry.CasterID);
                        var npc = NPCWorkerDelegate.GetNPCEntity(statusEntry.CasterID);
                        var monster = MonsterWorkerDelegate.GetNPCEntity(statusEntry.CasterID);
                        statusEntry.SourceEntity = (pc ?? npc) ?? monster;
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        var statusInfo = StatusEffectHelper.StatusInfo(statusEntry.StatusID);
                        if (statusInfo != null)
                        {
                            statusEntry.IsCompanyAction = statusInfo.CompanyAction;
                            var statusKey = statusInfo.Name.English;
                            switch (Settings.Default.GameLanguage)
                            {
                                case "French":
                                    statusKey = statusInfo.Name.French;
                                    break;
                                case "Japanese":
                                    statusKey = statusInfo.Name.Japanese;
                                    break;
                                case "German":
                                    statusKey = statusInfo.Name.German;
                                    break;
                                case "Chinese":
                                    statusKey = statusInfo.Name.Chinese;
                                    break;
                            }
                            statusEntry.StatusName = statusKey;
                        }
                    }
                    catch (Exception ex)
                    {
                        statusEntry.StatusName = "UNKNOWN";
                    }
                    if (statusEntry.IsValid())
                    {
                        entry.StatusEntries.Add(statusEntry);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            CleanXPValue(ref entry);

            if (isCurrentUser)
            {
                PCWorkerDelegate.CurrentUser = entry;
            }
            entry.CurrentUser = PCWorkerDelegate.CurrentUser;
            return entry;
        }

        private static void CleanXPValue(ref ActorEntity entity)
        {
            if (entity.HPCurrent < 0 || entity.HPMax < 0)
            {
                entity.HPCurrent = 1;
                entity.HPMax = 1;
            }
            if (entity.HPCurrent > entity.HPMax)
            {
                if (entity.HPMax == 0)
                {
                    entity.HPCurrent = 1;
                    entity.HPMax = 1;
                }
                else
                {
                    entity.HPCurrent = entity.HPMax;
                }
            }
            if (entity.MPCurrent < 0 || entity.MPMax < 0)
            {
                entity.MPCurrent = 1;
                entity.MPMax = 1;
            }
            if (entity.MPCurrent > entity.MPMax)
            {
                if (entity.MPMax == 0)
                {
                    entity.MPCurrent = 1;
                    entity.MPMax = 1;
                }
                else
                {
                    entity.MPCurrent = entity.MPMax;
                }
            }
            if (entity.GPCurrent < 0 || entity.GPMax < 0)
            {
                entity.GPCurrent = 1;
                entity.GPMax = 1;
            }
            if (entity.GPCurrent > entity.GPMax)
            {
                if (entity.GPMax == 0)
                {
                    entity.GPCurrent = 1;
                    entity.GPMax = 1;
                }
                else
                {
                    entity.GPCurrent = entity.GPMax;
                }
            }
            if (entity.CPCurrent < 0 || entity.CPMax < 0)
            {
                entity.CPCurrent = 1;
                entity.CPMax = 1;
            }
            if (entity.CPCurrent > entity.CPMax)
            {
                if (entity.CPMax == 0)
                {
                    entity.CPCurrent = 1;
                    entity.CPMax = 1;
                }
                else
                {
                    entity.CPCurrent = entity.CPMax;
                }
            }
        }
    }
}
