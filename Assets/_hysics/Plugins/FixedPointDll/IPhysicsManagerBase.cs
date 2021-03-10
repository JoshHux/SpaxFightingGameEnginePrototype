using System;
using Spax;

public interface IPhysicsManagerBase
{
	void Init();

	void UpdateStep();

	IWorld GetWorld();

	IWorldClone GetWorldClone();

	void RemoveBody(IBody iBody);
}
