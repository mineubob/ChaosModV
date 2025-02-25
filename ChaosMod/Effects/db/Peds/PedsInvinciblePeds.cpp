#include <stdafx.h>

#include "Effects/Register/RegisterEffect.h"

static void OnStop()
{
	for (auto ped : GetAllPeds())
		SET_ENTITY_INVINCIBLE(ped, false);
}

static void OnTick()
{
	for (auto ped : GetAllPeds())
		SET_ENTITY_INVINCIBLE(ped, true);
}

// clang-format off
REGISTER_EFFECT(nullptr, OnStop, OnTick, 
	{
		.Name = "Everyone Is Invincible",
		.Id = "peds_invincible",
		.IsTimed = true
	}
);