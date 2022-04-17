using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class ForzaConnection : MonoBehaviour
{
	public static ForzaConnection Instance;
	public ForzaDataPacket data;
	public int FORZA_DATA_OUT_PORT = 5300;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		data = new ForzaDataPacket();
		IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, FORZA_DATA_OUT_PORT);
		Task receiverTask = Task.Run(async () =>
		{
			UdpClient client = new UdpClient(FORZA_DATA_OUT_PORT);
			while (true)
			{
				await client.ReceiveAsync().ContinueWith(receive =>
				{
					byte[] resultBuffer = receive.Result.Buffer;
					if (!AdjustToBufferType(resultBuffer.Length))
					{
						return;
					}
					if (resultBuffer.IsRaceOn())
					{
						data = ParseData(resultBuffer);
					}
				});
			}
		});
	}

	private static ForzaDataPacket ParseData(byte[] packet)
	{
		ForzaDataPacket data = new ForzaDataPacket();

		data.IsRaceOn = packet.IsRaceOn();
		data.TimestampMS = packet.TimestampMs();
		data.EngineMaxRpm = packet.EngineMaxRpm();
		data.EngineIdleRpm = packet.EngineIdleRpm();
		data.CurrentEngineRpm = packet.CurrentEngineRpm();
		data.AccelerationX = packet.AccelerationX();
		data.AccelerationY = packet.AccelerationY();
		data.AccelerationZ = packet.AccelerationZ();
		data.VelocityX = packet.VelocityX();
		data.VelocityY = packet.VelocityY();
		data.VelocityZ = packet.VelocityZ();
		data.AngularVelocityX = packet.AngularVelocityX();
		data.AngularVelocityY = packet.AngularVelocityY();
		data.AngularVelocityZ = packet.AngularVelocityZ();
		data.Yaw = packet.Yaw();
		data.Pitch = packet.Pitch();
		data.Roll = packet.Roll();
		data.NormalizedSuspensionTravelFrontLeft = packet.NormSuspensionTravelFl();
		data.NormalizedSuspensionTravelFrontRight = packet.NormSuspensionTravelFr();
		data.NormalizedSuspensionTravelRearLeft = packet.NormSuspensionTravelRl();
		data.NormalizedSuspensionTravelRearRight = packet.NormSuspensionTravelRr();
		data.TireSlipRatioFrontLeft = packet.TireSlipRatioFl();
		data.TireSlipRatioFrontRight = packet.TireSlipRatioFr();
		data.TireSlipRatioRearLeft = packet.TireSlipRatioRl();
		data.TireSlipRatioRearRight = packet.TireSlipRatioRr();
		data.WheelRotationSpeedFrontLeft = packet.WheelRotationSpeedFl();
		data.WheelRotationSpeedFrontRight = packet.WheelRotationSpeedFr();
		data.WheelRotationSpeedRearLeft = packet.WheelRotationSpeedRl();
		data.WheelRotationSpeedRearRight = packet.WheelRotationSpeedRr();
		data.WheelOnRumbleStripFrontLeft = packet.WheelOnRumbleStripFl();
		data.WheelOnRumbleStripFrontRight = packet.WheelOnRumbleStripFr();
		data.WheelOnRumbleStripRearLeft = packet.WheelOnRumbleStripRl();
		data.WheelOnRumbleStripRearRight = packet.WheelOnRumbleStripRr();
		data.WheelInPuddleDepthFrontLeft = packet.WheelInPuddleFl();
		data.WheelInPuddleDepthFrontRight = packet.WheelInPuddleFr();
		data.WheelInPuddleDepthRearLeft = packet.WheelInPuddleRl();
		data.WheelInPuddleDepthRearRight = packet.WheelInPuddleRr();
		data.SurfaceRumbleFrontLeft = packet.SurfaceRumbleFl();
		data.SurfaceRumbleFrontRight = packet.SurfaceRumbleFr();
		data.SurfaceRumbleRearLeft = packet.SurfaceRumbleRl();
		data.SurfaceRumbleRearRight = packet.SurfaceRumbleRr();
		data.TireSlipAngleFrontLeft = packet.TireSlipAngleFl();
		data.TireSlipAngleFrontRight = packet.TireSlipAngleFr();
		data.TireSlipAngleRearLeft = packet.TireSlipAngleRl();
		data.TireSlipAngleRearRight = packet.TireSlipAngleRr();
		data.TireCombinedSlipFrontLeft = packet.TireCombinedSlipFl();
		data.TireCombinedSlipFrontRight = packet.TireCombinedSlipFr();
		data.TireCombinedSlipRearLeft = packet.TireCombinedSlipRl();
		data.TireCombinedSlipRearRight = packet.TireCombinedSlipRr();
		data.SuspensionTravelMetersFrontLeft = packet.SuspensionTravelMetersFl();
		data.SuspensionTravelMetersFrontRight = packet.SuspensionTravelMetersFr();
		data.SuspensionTravelMetersRearLeft = packet.SuspensionTravelMetersRl();
		data.SuspensionTravelMetersRearRight = packet.SuspensionTravelMetersRr();
		data.CarOrdinal = packet.CarOrdinal();
		data.CarClass = packet.CarClass();
		data.CarPerformanceIndex = packet.CarPerformanceIndex();
		data.DrivetrainType = packet.DriveTrain();
		data.NumCylinders = packet.NumCylinders();

		data.PositionX = packet.PositionX();
		data.PositionY = packet.PositionY();
		data.PositionZ = packet.PositionZ();
		data.Speed = packet.Speed();
		data.Power = packet.Power();
		data.Torque = packet.Torque();
		data.TireTempFl = packet.TireTempFl();
		data.TireTempFr = packet.TireTempFr();
		data.TireTempRl = packet.TireTempRl();
		data.TireTempRr = packet.TireTempRr();
		data.Boost = packet.Boost();
		data.Fuel = packet.Fuel();
		data.Distance = packet.Distance();
		data.BestLapTime = packet.BestLapTime();
		data.LastLapTime = packet.LastLapTime();
		data.CurrentLapTime = packet.CurrentLapTime();
		data.CurrentRaceTime = packet.CurrentRaceTime();
		data.Lap = packet.Lap();
		data.RacePosition = packet.RacePosition();
		data.Accelerator = packet.Accelerator();
		data.Brake = packet.Brake();
		data.Clutch = packet.Clutch();
		data.Handbrake = packet.Handbrake();
		data.Gear = packet.Gear();
		data.Steer = packet.Steer();
		data.NormalDrivingLine = packet.NormalDrivingLine();
		data.NormalAiBrakeDifference = packet.NormalAiBrakeDifference();

		return data;
	}

	private static bool AdjustToBufferType(int bufferLength)
	{
		switch (bufferLength)
		{
			case 232: // FM7 sled
				return false;
			case 311: // FM7 dash
				ForzaData.BufferOffset = 0;
				return true;
			case 324: // FH4
				ForzaData.BufferOffset = 12;
				return true;
			default:
				return false;
		}
	}
}
