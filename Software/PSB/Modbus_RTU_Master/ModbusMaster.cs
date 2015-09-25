using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Modbus
{
    public enum Function {  WriteMultiple = 0x10};
    public class ModbusMaster
    {
       


        #region Constructor / Deconstructor
        public ModbusMaster()
        {
           

        }

      
        ~ModbusMaster()
        {
        }
        #endregion



        


      


        #region CRC Computation
        private void ComputeCRC(List<Byte> message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Count); i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Packetize
        public List<Byte> Packetize(byte slaveID, Function function, ushort regAddress, ushort registers, List<byte> data)
        {
            List<Byte> ModbusPacket = new List<byte>();
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            ModbusPacket.Add((byte) slaveID);
            ModbusPacket.Add((byte)function);
            ModbusPacket.Add((byte)(regAddress >> 8));
            ModbusPacket.Add((byte)regAddress);
            ModbusPacket.Add((byte)(registers >> 8));
            ModbusPacket.Add((byte)registers);
            ModbusPacket.Add((byte)(registers * 2));

            ModbusPacket.AddRange(data);


            ComputeCRC(ModbusPacket, ref CRC);
            ModbusPacket.Add(CRC[0]);
            ModbusPacket.Add(CRC[1]);

            return ModbusPacket;
        }
        #endregion

        public List<Byte> Parse(List<Byte> packet)
        {
            return null;
        }

        #region Check Response
        private bool CheckResponse(List<Byte> response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            ComputeCRC(response, ref CRC);
            if (CRC[0] == response[response.Count - 2] && CRC[1] == response[response.Count - 1])
                return true;
            else
                return false;
        }
        #endregion

       

       

       

    }
}
