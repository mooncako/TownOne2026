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
        static const AkUniqueID AMB_PLAY = 2381502256U;
        static const AkUniqueID COLL_BALL = 1627453353U;
        static const AkUniqueID COLL_BUMPERS = 2883100974U;
        static const AkUniqueID COLL_MINION = 773862064U;
        static const AkUniqueID EMIT_POWERUP = 2031107581U;
        static const AkUniqueID EMIT_SCORE = 2401631563U;
        static const AkUniqueID EMIT_SHOP = 2436315639U;
        static const AkUniqueID MUS_PLAY = 1280297747U;
        static const AkUniqueID OBJ_MINION_DIE = 2792663994U;
        static const AkUniqueID OBJ_MINION_SET = 2862290544U;
        static const AkUniqueID PLYR_PADDLE_MOVE = 3258129853U;
        static const AkUniqueID UI_CONFIRM_GENERIC = 4060032068U;
        static const AkUniqueID UI_CONFIRM_PLAY = 2428166787U;
        static const AkUniqueID UI_CONFIRM_QUITGAME = 2912873858U;
        static const AkUniqueID UI_HIGHLIGHT = 1402340918U;
        static const AkUniqueID UI_PAUSEGAME = 3783962382U;
        static const AkUniqueID UI_RESUMEGAME = 3020718913U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace GAMESTATE
        {
            static const AkUniqueID GROUP = 4091656514U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSE = 3092587493U;
            } // namespace STATE
        } // namespace GAMESTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace MINIONSIZE
        {
            static const AkUniqueID GROUP = 3068666232U;

            namespace SWITCH
            {
                static const AkUniqueID HEAVY = 2732489590U;
                static const AkUniqueID MED = 981339021U;
                static const AkUniqueID SMALL = 1846755610U;
            } // namespace SWITCH
        } // namespace MINIONSIZE

        namespace MINIONTYPE
        {
            static const AkUniqueID GROUP = 4233847531U;

            namespace SWITCH
            {
                static const AkUniqueID HEAVEN = 3460057008U;
                static const AkUniqueID HELL = 3632828376U;
            } // namespace SWITCH
        } // namespace MINIONTYPE

        namespace POWERUPS
        {
            static const AkUniqueID GROUP = 2600048462U;

            namespace SWITCH
            {
                static const AkUniqueID P1 = 1635194252U;
                static const AkUniqueID P2 = 1635194255U;
                static const AkUniqueID P3 = 1635194254U;
            } // namespace SWITCH
        } // namespace POWERUPS

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID TO26_SB = 3575583478U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MAIN_AUDIO_BUS = 2246998526U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
