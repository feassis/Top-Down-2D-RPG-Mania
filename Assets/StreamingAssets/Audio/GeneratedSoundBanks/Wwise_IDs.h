/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_LEVEL_GAME_COMPLETE = 322272001U;
        static const AkUniqueID PLAY_MAIN_MENU = 3306210749U;
        static const AkUniqueID PLAY_VILLAGE_COMBAT = 1840655293U;
        static const AkUniqueID STOP_LEVEL_GAME_COMPLETE = 2459528395U;
        static const AkUniqueID STOP_MAIN_MENU = 774860123U;
        static const AkUniqueID STOP_VILLAGE_COMBAT = 2050223323U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace LEVEL_GAME_COMPLETE
        {
            static const AkUniqueID GROUP = 1139417068U;

            namespace STATE
            {
                static const AkUniqueID GAME_COMPLETE = 1343734759U;
                static const AkUniqueID LEVEL_COMPLETE = 3736098925U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace LEVEL_GAME_COMPLETE

        namespace VILLAGE_COMBAT
        {
            static const AkUniqueID GROUP = 4149808090U;

            namespace STATE
            {
                static const AkUniqueID COMBAT_CHIEF = 432693839U;
                static const AkUniqueID COMBAT_FINAL = 1503069520U;
                static const AkUniqueID COMBAT_NORMAL = 3178003625U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID VILLAGE = 3945572659U;
            } // namespace STATE
        } // namespace VILLAGE_COMBAT

    } // namespace STATES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
