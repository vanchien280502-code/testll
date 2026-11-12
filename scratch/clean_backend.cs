using System;
using System.IO;
using System.Text;

class RevertAndClean {
    static void Main() {
        string path = @"QLTK\NativeBackend.cs";
        string text = File.ReadAllText(path, Encoding.UTF8);

        // 1. Replace TaskPacket and TaskPacketCore
        string oldTpMarker = "internal static byte[] TaskPacket(long data,long game){";
        int tpStart = text.IndexOf(oldTpMarker);
        if (tpStart < 0) throw new Exception("TaskPacket not found");

        string coreEndMarker = "a.Label(\"done\");a.Label(\"return\");a.Emit(0x48,0x83,0xc4,0x28,0x5e,0x5b,0xc3);return a.Finish();\r\n        }";
        if (!text.Contains(coreEndMarker)) {
            coreEndMarker = "a.Label(\"done\");a.Label(\"return\");a.Emit(0x48,0x83,0xc4,0x28,0x5e,0x5b,0xc3);return a.Finish();\n        }";
        }
        int tpEnd = text.IndexOf(coreEndMarker, tpStart);
        if (tpEnd < 0) throw new Exception("coreEndMarker not found");
        tpEnd += coreEndMarker.Length;

        string cleanTaskPacket = @"internal static byte[] TaskPacket(long data,long game){return TaskPacketCore(data,game+0x16ee820,game+0x16f760,game+0x2e2aa0,game+0x1dd900);}
        internal static byte[] TaskPacketCore(long data,long serviceSlot,long allocate,long ctor,long initialize){
            Code64 a=new Code64();a.Emit(0x53,0x56,0x48,0x83,0xec,0x28,0x48,0xbb);a.Q(data);
            a.Emit(0x83,0x3b,1);a.J(0x85,""return"");a.Emit(0xc7,0x03,2,0,0,0);
            Slot(a,serviceSlot,""failed"",initialize);a.Emit(0x48,0x89,0xc1);a.Call(allocate);
            a.Emit(0x48,0x85,0xc0);a.J(0x84,""failed"");
            a.Emit(0x48,0x89,0xc6,0x48,0x89,0xc1,0x8b,0x53,4,0x45,0x33,0xc0);a.Call(ctor);
            a.Emit(0x83,0x7b,8,0);a.J(0x84,""send"");
            a.Emit(0x83,0x7b,8,2);a.J(0x84,""write_short"");
            a.Emit(0x83,0x7b,8,3);a.J(0x84,""turnin_item"");
            a.Emit(0x83,0x7b,8,4);a.J(0x84,""two_shorts"");
            a.Emit(0x83,0x7b,8,5);a.J(0x84,""short_byte"");
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x30,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x28,2,0,0);
            a.Jump(""send"");
            a.Label(""write_short"");
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
            a.Jump(""send"");
            a.Label(""turnin_item"");
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x30,2,0,0,0x48,0x89,0xf1,0xba,1,0,0,0,0xff,0x90,0x28,2,0,0);
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
            a.Jump(""send"");
            a.Label(""two_shorts"");
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,16,0xff,0x90,0x48,2,0,0);
            a.Jump(""send"");
            a.Label(""short_byte"");
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x50,2,0,0,0x48,0x89,0xf1,0x8b,0x53,12,0xff,0x90,0x48,2,0,0);
            a.Emit(0x48,0x8b,0x06,0x4c,0x8b,0x80,0x20,2,0,0,0x48,0x89,0xf1,0x8b,0x53,16,0xff,0x90,0x18,2,0,0);
            a.Label(""send"");
            a.Emit(0x48,0x8b,0x06,0x48,0x8b,0x90,0xa0,2,0,0,0x48,0x89,0xf1,0xff,0x90,0x98,2,0,0,0xc7,0x03,0,0,0,0);
            a.Jump(""done"");a.Label(""failed"");a.Emit(0xc7,0x03,3,0,0,0);
            a.Label(""done"");a.Label(""return"");a.Emit(0x48,0x83,0xc4,0x28,0x5e,0x5b,0xc3);return a.Finish();
        }";

        text = text.Substring(0, tpStart) + cleanTaskPacket + text.Substring(tpEnd);
        Console.WriteLine("1. Cleaned TaskPacket and TaskPacketCore");

        // 2. Clean PumpLuckyWheel
        string oldPumpStart = "internal static void PumpLuckyWheel(Remote r,long taskPacketData,long game,NativeJob j,Func<InventorySnapshot> bagReader=null,DateTime? clock=null){";
        string nextMethod = "static long ResolvedClass(Remote r,long slot){";
        int pStart = text.IndexOf(oldPumpStart);
        if (pStart < 0) throw new Exception("PumpLuckyWheel not found");
        int pEnd = text.IndexOf(nextMethod, pStart);
        if (pEnd < 0) throw new Exception("nextMethod not found");

        string cleanPump = @"internal static void PumpLuckyWheel(Remote r,long taskPacketData,long game,NativeJob j,Func<InventorySnapshot> bagReader=null,DateTime? clock=null){
            if(!j.LuckyWheelEnabled||j.CloseRequested||j.CancelRequested||!j.TrainReady)return;
            DateTime now=clock??DateTime.UtcNow;
            if(j.TaskPacketOpcode>0||j.FilterDropQueue.Count>0){
                j.LuckyWheelStatus=""Đang vứt đồ rác (""+j.FilterDropQueue.Count+"" món)..."";
                return;
            }
            if(now<j.LuckyWheelNext)return;
            InventorySnapshot bag=bagReader==null?j.Inventory:bagReader();
            if(bag==null||!bag.Ready||bag.Capacity<=0){
                try{bag=ReadInventorySnapshot(r,game);j.Inventory=bag;}catch{j.LuckyWheelStatus=""Đợi đọc hành trang..."";return;}
            }
            int freeSlots=bag.Capacity-bag.Items.Length;
            if(freeSlots<j.LuckyWheelMinFreeSlots){
                j.LuckyWheelStatus=""Túi đầy (còn ""+freeSlots+"" ô trống) • Đợi lọc đồ vứt..."";
                j.LuckyWheelNext=now.AddMilliseconds(300);
                return;
            }
            int ticketCount=CountTickets(bag,j.LuckyWheelTicketIds);
            j.LuckyWheelLastTicketTotal=ticketCount;
            if(j.LuckyWheelOnlyTicket&&ticketCount<=0){
                j.LuckyWheelEnabled=false;
                j.LuckyWheelStatus=""Hết Vé may mắn • Tự dừng an toàn (đã quay ""+j.LuckyWheelSpunCount+"" lượt)"";
                Log(j,""lucky_wheel_stopped_no_tickets"","",""\""spun\"":""+j.LuckyWheelSpunCount);
                return;
            }

            // Tự động phát hiện và chọn menu 'Chọn loại vòng quay' khi xuất hiện
            string[] opts=j.MenuOptions;
            if(opts!=null&&opts.Length>0){
                int targetIdx=-1;
                for(int i=0;i<opts.Length;i++){
                    if(opts[i].IndexOf(""vòng quay"",StringComparison.OrdinalIgnoreCase)>=0||
                       opts[i].IndexOf(""vận may"",StringComparison.OrdinalIgnoreCase)>=0){
                        if(j.LuckyWheelType==1&&opts[i].IndexOf(""bạc"",StringComparison.OrdinalIgnoreCase)>=0){targetIdx=i;break;}
                        if(j.LuckyWheelType==0&&(opts[i].IndexOf(""vàng"",StringComparison.OrdinalIgnoreCase)>=0||opts[i].IndexOf(""vé"",StringComparison.OrdinalIgnoreCase)>=0)){targetIdx=i;break;}
                    }
                }
                if(targetIdx>=0){
                    SelectDynamicMenuIndex(j.Id,targetIdx);
                    j.LuckyWheelNext=now.AddMilliseconds(600);
                    j.LuckyWheelStatus=""Đã tự chọn ""+opts[targetIdx]+"" • Đang mở bảng quay..."";
                    Log(j,""lucky_wheel_menu_selected"","",""\""index\"":""+targetIdx);
                    return;
                }
            }

            // Gửi lệnh quay (x10 hoặc x1)
            int batch=j.LuckyWheelBatch;
            if(j.LuckyWheelOnlyTicket&&ticketCount<batch){
                batch=1;
            }
            int subOpcode=(j.LuckyWheelType==0)?(batch==10?0x9A:0x9C):(batch==10?0x99:0x9B);
            SendTaskPacket(j.Id,0x85,subOpcode,1);
            j.LuckyWheelSpunCount+=batch;
            j.LuckyWheelNext=now.AddMilliseconds(j.LuckyWheelIntervalMs);
            j.LuckyWheelStatus=""Đang quay (""+(j.LuckyWheelType==0?""Vàng"":""Bạc"")+"" x""+batch+"") • Còn ""+ticketCount+"" vé • Đã quay ""+j.LuckyWheelSpunCount+"" lượt"";
            Log(j,""lucky_wheel_spun"","",""\""type\"":""+j.LuckyWheelType+"",""\""batch\"":""+batch+"",""\""sub_opcode\"":""+subOpcode+"",""\""remaining_tickets\"":""+ticketCount);
        }
        ";

        text = text.Substring(0, pStart) + cleanPump + text.Substring(pEnd);
        Console.WriteLine("2. Cleaned PumpLuckyWheel");

        File.WriteAllText(path, text, new UTF8Encoding(true));
        Console.WriteLine("Success!");
    }
}

