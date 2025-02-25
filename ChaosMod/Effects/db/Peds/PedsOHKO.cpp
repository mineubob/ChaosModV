#include <stdafx.h>

#include "Effects/Register/RegisterEffect.h"

static void OnStop()
{
	SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(PLAYER_ID(), 1.f);

	for (Ped ped : GetAllPeds())
		if (!IS_PED_DEAD_OR_DYING(ped, true))
			SET_ENTITY_HEALTH(ped, GET_PED_MAX_HEALTH(ped), 0);
}

static void OnTick()
{
	Hash playerHash = GET_ENTITY_MODEL(PLAYER_PED_ID());
	// trevor
	if (playerHash == 2608926626)
		_SET_SPECIAL_ABILITY(PLAYER_ID(), 0, 0);

	SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(PLAYER_ID(), .0f);

	for (Ped ped : GetAllPeds())
	{
		if (!IS_PED_DEAD_OR_DYING(ped, true) && GET_ENTITY_HEALTH(ped) > 101)
		{
			SET_ENTITY_HEALTH(ped, 101, 0);
			SET_PED_ARMOUR(ped, 0);
		}
	}
}

// clang-format off
REGISTER_EFFECT(nullptr, OnStop, OnTick, 
	{
		.Name = "One Hit KO",
		.Id = "player_ohko",
		.IsTimed = true,
		.IncompatibleWith = { "player_bees" }
	}
);