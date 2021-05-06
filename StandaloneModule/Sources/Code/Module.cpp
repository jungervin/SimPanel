#include <MSFS\MSFS.h>
#include <MSFS\MSFS_WindowsTypes.h>
#include <MSFS\Legacy\gauges.h>
#include <SimConnect.h>

#include "Module.h"

#include <string>
using namespace std;

HANDLE g_hSimConnect;

enum eEvents
{
	EVENT_FLIGHT_LOADED,
	SIMPANEL_FLAPS_INCR,
	SIMPANEL_FLAPS_DECR,
};


static enum GROUP_ID {
	GROUP0,
};

std::string events[272][2] = {
	{"SP_AS1000_PFD_VOL_1_INC","(>H:AS1000_PFD_VOL_1_INC)"},
	{"SP_AS1000_PFD_VOL_1_DEC","(>H:AS1000_PFD_VOL_1_DEC)"},
	{"SP_AS1000_PFD_VOL_2_INC","(>H:AS1000_PFD_VOL_2_INC)"},
	{"SP_AS1000_PFD_VOL_2_DEC","(>H:AS1000_PFD_VOL_2_DEC)"},
	{"SP_AS1000_PFD_NAV_Switch","(>H:AS1000_PFD_NAV_Switch)"},
	{"SP_AS1000_PFD_NAV_Large_INC","(>H:AS1000_PFD_NAV_Large_INC)"},
	{"SP_AS1000_PFD_NAV_Large_DEC","(>H:AS1000_PFD_NAV_Large_DEC)"},
	{"SP_AS1000_PFD_NAV_Small_INC","(>H:AS1000_PFD_NAV_Small_INC)"},
	{"SP_AS1000_PFD_NAV_Small_DEC","(>H:AS1000_PFD_NAV_Small_DEC)"},
	{"SP_AS1000_PFD_NAV_Push","(>H:AS1000_PFD_NAV_Push)"},
	{"SP_AS1000_PFD_COM_Switch_Long","(>H:AS1000_PFD_COM_Switch_Long)"},
	{"SP_AS1000_PFD_COM_Switch","(>H:AS1000_PFD_COM_Switch)"},
	{"SP_AS1000_PFD_COM_Large_INC","(>H:AS1000_PFD_COM_Large_INC)"},
	{"SP_AS1000_PFD_COM_Large_DEC","(>H:AS1000_PFD_COM_Large_DEC)"},
	{"SP_AS1000_PFD_COM_Small_INC","(>H:AS1000_PFD_COM_Small_INC)"},
	{"SP_AS1000_PFD_COM_Small_DEC","(>H:AS1000_PFD_COM_Small_DEC)"},
	{"SP_AS1000_PFD_COM_Push","(>H:AS1000_PFD_COM_Push)"},
	{"SP_AS1000_PFD_BARO_INC","(>H:AS1000_PFD_BARO_INC)"},
	{"SP_AS1000_PFD_BARO_DEC","(>H:AS1000_PFD_BARO_DEC)"},
	{"SP_AS1000_PFD_CRS_INC","(>H:AS1000_PFD_CRS_INC)"},
	{"SP_AS1000_PFD_CRS_DEC","(>H:AS1000_PFD_CRS_DEC)"},
	{"SP_AS1000_PFD_CRS_PUSH","(>H:AS1000_PFD_CRS_PUSH)"},
	{"SP_AS1000_PFD_SOFTKEYS_1","(>H:AS1000_PFD_SOFTKEYS_1)"},
	{"SP_AS1000_PFD_SOFTKEYS_2","(>H:AS1000_PFD_SOFTKEYS_2)"},
	{"SP_AS1000_PFD_SOFTKEYS_3","(>H:AS1000_PFD_SOFTKEYS_3)"},
	{"SP_AS1000_PFD_SOFTKEYS_4","(>H:AS1000_PFD_SOFTKEYS_4)"},
	{"SP_AS1000_PFD_SOFTKEYS_5","(>H:AS1000_PFD_SOFTKEYS_5)"},
	{"SP_AS1000_PFD_SOFTKEYS_6","(>H:AS1000_PFD_SOFTKEYS_6)"},
	{"SP_AS1000_PFD_SOFTKEYS_7","(>H:AS1000_PFD_SOFTKEYS_7)"},
	{"SP_AS1000_PFD_SOFTKEYS_8","(>H:AS1000_PFD_SOFTKEYS_8)"},
	{"SP_AS1000_PFD_SOFTKEYS_9","(>H:AS1000_PFD_SOFTKEYS_9)"},
	{"SP_AS1000_PFD_SOFTKEYS_10","(>H:AS1000_PFD_SOFTKEYS_10)"},
	{"SP_AS1000_PFD_SOFTKEYS_11","(>H:AS1000_PFD_SOFTKEYS_11)"},
	{"SP_AS1000_PFD_SOFTKEYS_12","(>H:AS1000_PFD_SOFTKEYS_12)"},
	{"SP_AS1000_PFD_DIRECTTO","(>H:AS1000_PFD_DIRECTTO)"},
	{"SP_AS1000_PFD_ENT_Push","(>H:AS1000_PFD_ENT_Push)"},
	{"SP_AS1000_PFD_CLR_Long","(>H:AS1000_PFD_CLR_Long)"},
	{"SP_AS1000_PFD_CLR","(>H:AS1000_PFD_CLR)"},
	{"SP_AS1000_PFD_MENU_Push","(>H:AS1000_PFD_MENU_Push)"},
	{"SP_AS1000_PFD_FPL_Push","(>H:AS1000_PFD_FPL_Push)"},
	{"SP_AS1000_PFD_PROC_Push","(>H:AS1000_PFD_PROC_Push)"},
	{"SP_AS1000_PFD_FMS_Upper_INC","(>H:AS1000_PFD_FMS_Upper_INC)"},
	{"SP_AS1000_PFD_FMS_Upper_DEC","(>H:AS1000_PFD_FMS_Upper_DEC)"},
	{"SP_AS1000_PFD_FMS_Upper_PUSH","(>H:AS1000_PFD_FMS_Upper_PUSH)"},
	{"SP_AS1000_PFD_FMS_Lower_INC","(>H:AS1000_PFD_FMS_Lower_INC)"},
	{"SP_AS1000_PFD_FMS_Lower_DEC","(>H:AS1000_PFD_FMS_Lower_DEC)"},
	{"SP_AS1000_PFD_RANGE_INC","(>H:AS1000_PFD_RANGE_INC)"},
	{"SP_AS1000_PFD_RANGE_DEC","(>H:AS1000_PFD_RANGE_DEC)"},
	{"SP_AS1000_PFD_JOYSTICK_PUSH","(>H:AS1000_PFD_JOYSTICK_PUSH)"},
	{"SP_AS1000_PFD_ActivateMapCursor","(>H:AS1000_PFD_ActivateMapCursor)"},
	{"SP_AS1000_PFD_DeactivateMapCursor","(>H:AS1000_PFD_DeactivateMapCursor)"},
	{"SP_AS1000_PFD_JOYSTICK_RIGHT","(>H:AS1000_PFD_JOYSTICK_RIGHT)"},
	{"SP_AS1000_PFD_JOYSTICK_LEFT","(>H:AS1000_PFD_JOYSTICK_LEFT)"},
	{"SP_AS1000_PFD_JOYSTICK_UP","(>H:AS1000_PFD_JOYSTICK_UP)"},
	{"SP_AS1000_PFD_JOYSTICK_DOWN","(>H:AS1000_PFD_JOYSTICK_DOWN)"},
	{"SP_AS1000_MFD_VOL_1_INC","(>H:AS1000_MFD_VOL_1_INC)"},
	{"SP_AS1000_MFD_VOL_1_DEC","(>H:AS1000_MFD_VOL_1_DEC)"},
	{"SP_AS1000_MFD_VOL_2_INC","(>H:AS1000_MFD_VOL_2_INC)"},
	{"SP_AS1000_MFD_VOL_2_DEC","(>H:AS1000_MFD_VOL_2_DEC)"},
	{"SP_AS1000_MFD_NAV_Switch","(>H:AS1000_MFD_NAV_Switch)"},
	{"SP_AS1000_MFD_NAV_Large_INC","(>H:AS1000_MFD_NAV_Large_INC)"},
	{"SP_AS1000_MFD_NAV_Large_DEC","(>H:AS1000_MFD_NAV_Large_DEC)"},
	{"SP_AS1000_MFD_NAV_Small_INC","(>H:AS1000_MFD_NAV_Small_INC)"},
	{"SP_AS1000_MFD_NAV_Small_DEC","(>H:AS1000_MFD_NAV_Small_DEC)"},
	{"SP_AS1000_MFD_NAV_Push","(>H:AS1000_MFD_NAV_Push)"},
	{"SP_AS1000_MFD_COM_Switch_Long","(>H:AS1000_MFD_COM_Switch_Long)"},
	{"SP_AS1000_MFD_COM_Switch","(>H:AS1000_MFD_COM_Switch)"},
	{"SP_AS1000_MFD_COM_Large_INC","(>H:AS1000_MFD_COM_Large_INC)"},
	{"SP_AS1000_MFD_COM_Large_DEC","(>H:AS1000_MFD_COM_Large_DEC)"},
	{"SP_AS1000_MFD_COM_Small_INC","(>H:AS1000_MFD_COM_Small_INC)"},
	{"SP_AS1000_MFD_COM_Small_DEC","(>H:AS1000_MFD_COM_Small_DEC)"},
	{"SP_AS1000_MFD_COM_Push","(>H:AS1000_MFD_COM_Push)"},
	{"SP_AS1000_MFD_BARO_INC","(>H:AS1000_MFD_BARO_INC)"},
	{"SP_AS1000_MFD_BARO_DEC","(>H:AS1000_MFD_BARO_DEC)"},
	{"SP_AS1000_MFD_CRS_INC","(>H:AS1000_MFD_CRS_INC)"},
	{"SP_AS1000_MFD_CRS_DEC","(>H:AS1000_MFD_CRS_DEC)"},
	{"SP_AS1000_MFD_CRS_PUSH","(>H:AS1000_MFD_CRS_PUSH)"},
	{"SP_AS1000_MFD_SOFTKEYS_1","(>H:AS1000_MFD_SOFTKEYS_1)"},
	{"SP_AS1000_MFD_SOFTKEYS_2","(>H:AS1000_MFD_SOFTKEYS_2)"},
	{"SP_AS1000_MFD_SOFTKEYS_3","(>H:AS1000_MFD_SOFTKEYS_3)"},
	{"SP_AS1000_MFD_SOFTKEYS_4","(>H:AS1000_MFD_SOFTKEYS_4)"},
	{"SP_AS1000_MFD_SOFTKEYS_5","(>H:AS1000_MFD_SOFTKEYS_5)"},
	{"SP_AS1000_MFD_SOFTKEYS_6","(>H:AS1000_MFD_SOFTKEYS_6)"},
	{"SP_AS1000_MFD_SOFTKEYS_7","(>H:AS1000_MFD_SOFTKEYS_7)"},
	{"SP_AS1000_MFD_SOFTKEYS_8","(>H:AS1000_MFD_SOFTKEYS_8)"},
	{"SP_AS1000_MFD_SOFTKEYS_9","(>H:AS1000_MFD_SOFTKEYS_9)"},
	{"SP_AS1000_MFD_SOFTKEYS_10","(>H:AS1000_MFD_SOFTKEYS_10)"},
	{"SP_AS1000_MFD_SOFTKEYS_11","(>H:AS1000_MFD_SOFTKEYS_11)"},
	{"SP_AS1000_MFD_SOFTKEYS_12","(>H:AS1000_MFD_SOFTKEYS_12)"},
	{"SP_AS1000_MFD_DIRECTTO","(>H:AS1000_MFD_DIRECTTO)"},
	{"SP_AS1000_MFD_ENT_Push","(>H:AS1000_MFD_ENT_Push)"},
	{"SP_AS1000_MFD_CLR_Long","(>H:AS1000_MFD_CLR_Long)"},
	{"SP_AS1000_MFD_CLR","(>H:AS1000_MFD_CLR)"},
	{"SP_AS1000_MFD_MENU_Push","(>H:AS1000_MFD_MENU_Push)"},
	{"SP_AS1000_MFD_FPL_Push","(>H:AS1000_MFD_FPL_Push)"},
	{"SP_AS1000_MFD_PROC_Push","(>H:AS1000_MFD_PROC_Push)"},
	{"SP_AS1000_MFD_FMS_Upper_INC","(>H:AS1000_MFD_FMS_Upper_INC)"},
	{"SP_AS1000_MFD_FMS_Upper_DEC","(>H:AS1000_MFD_FMS_Upper_DEC)"},
	{"SP_AS1000_MFD_FMS_Upper_PUSH","(>H:AS1000_MFD_FMS_Upper_PUSH)"},
	{"SP_AS1000_MFD_FMS_Lower_INC","(>H:AS1000_MFD_FMS_Lower_INC)"},
	{"SP_AS1000_MFD_FMS_Lower_DEC","(>H:AS1000_MFD_FMS_Lower_DEC)"},
	{"SP_AS1000_MFD_RANGE_INC","(>H:AS1000_MFD_RANGE_INC)"},
	{"SP_AS1000_MFD_RANGE_DEC","(>H:AS1000_MFD_RANGE_DEC)"},
	{"SP_AS1000_MFD_JOYSTICK_PUSH","(>H:AS1000_MFD_JOYSTICK_PUSH)"},
	{"SP_AS1000_MFD_ActivateMapCursor","(>H:AS1000_MFD_ActivateMapCursor)"},
	{"SP_AS1000_MFD_DeactivateMapCursor","(>H:AS1000_MFD_DeactivateMapCursor)"},
	{"SP_AS1000_MFD_JOYSTICK_RIGHT","(>H:AS1000_MFD_JOYSTICK_RIGHT)"},
	{"SP_AS1000_MFD_JOYSTICK_LEFT","(>H:AS1000_MFD_JOYSTICK_LEFT)"},
	{"SP_AS1000_MFD_JOYSTICK_UP","(>H:AS1000_MFD_JOYSTICK_UP)"},
	{"SP_AS1000_MFD_JOYSTICK_DOWN","(>H:AS1000_MFD_JOYSTICK_DOWN)"},
	{"SP_AS1000_MID_COM_1_Push","(>H:AS1000_MID_COM_1_Push)"},
	{"SP_AS1000_MID_COM_2_Push","(>H:AS1000_MID_COM_2_Push)"},
	{"SP_AS1000_MID_COM_3_Push","(>H:AS1000_MID_COM_3_Push)"},
	{"SP_AS1000_MID_COM_Mic_1_Push","(>H:AS1000_MID_COM_Mic_1_Push)"},
	{"SP_AS1000_MID_COM_Mic_2_Push","(>H:AS1000_MID_COM_Mic_2_Push)"},
	{"SP_AS1000_MID_COM_Mic_3_Push","(>H:AS1000_MID_COM_Mic_3_Push)"},
	{"SP_AS1000_MID_COM_Swap_1_2_Push","(>H:AS1000_MID_COM_Swap_1_2_Push)"},
	{"SP_AS1000_MID_TEL_Push","(>H:AS1000_MID_TEL_Push)"},
	{"SP_AS1000_MID_PA_Push","(>H:AS1000_MID_PA_Push)"},
	{"SP_AS1000_MID_SPKR_Push","(>H:AS1000_MID_SPKR_Push)"},
	{"SP_AS1000_MID_MKR_Mute_Push","(>H:AS1000_MID_MKR_Mute_Push)"},
	{"SP_AS1000_MID_HI_SENS_Push","(>H:AS1000_MID_HI_SENS_Push)"},
	{"SP_AS1000_MID_DME_Push","(>H:AS1000_MID_DME_Push)"},
	{"SP_AS1000_MID_NAV_1_Push","(>H:AS1000_MID_NAV_1_Push)"},
	{"SP_AS1000_MID_NAV_2_Push","(>H:AS1000_MID_NAV_2_Push)"},
	{"SP_AS1000_MID_ADF_Push","(>H:AS1000_MID_ADF_Push)"},
	{"SP_AS1000_MID_AUX_Push","(>H:AS1000_MID_AUX_Push)"},
	{"SP_AS1000_MID_MAN_SQ_Push","(>H:AS1000_MID_MAN_SQ_Push)"},
	{"SP_AS1000_MID_Play_Push","(>H:AS1000_MID_Play_Push)"},
	{"SP_AS1000_MID_Isolate_Pilot_Push","(>H:AS1000_MID_Isolate_Pilot_Push)"},
	{"SP_AS1000_MID_Isolate_Copilot_Push","(>H:AS1000_MID_Isolate_Copilot_Push)"},
	{"SP_AS1000_MID_Pass_Copilot_INC","(>H:AS1000_MID_Pass_Copilot_INC)"},
	{"SP_AS1000_MID_Pass_Copilot_DEC","(>H:AS1000_MID_Pass_Copilot_DEC)"},
	{"SP_AS1000_MID_Display_Backup_Push","(>H:AS1000_MID_Display_Backup_Push)"},
	{"SP_AS3000_PFD_SOFTKEYS_1","(>H:AS3000_PFD_SOFTKEYS_1)"},
	{"SP_AS3000_PFD_SOFTKEYS_2","(>H:AS3000_PFD_SOFTKEYS_2)"},
	{"SP_AS3000_PFD_SOFTKEYS_3","(>H:AS3000_PFD_SOFTKEYS_3)"},
	{"SP_AS3000_PFD_SOFTKEYS_4","(>H:AS3000_PFD_SOFTKEYS_4)"},
	{"SP_AS3000_PFD_SOFTKEYS_5","(>H:AS3000_PFD_SOFTKEYS_5)"},
	{"SP_AS3000_PFD_SOFTKEYS_6","(>H:AS3000_PFD_SOFTKEYS_6)"},
	{"SP_AS3000_PFD_SOFTKEYS_7","(>H:AS3000_PFD_SOFTKEYS_7)"},
	{"SP_AS3000_PFD_SOFTKEYS_8","(>H:AS3000_PFD_SOFTKEYS_8)"},
	{"SP_AS3000_PFD_SOFTKEYS_9","(>H:AS3000_PFD_SOFTKEYS_9)"},
	{"SP_AS3000_PFD_SOFTKEYS_10","(>H:AS3000_PFD_SOFTKEYS_10)"},
	{"SP_AS3000_PFD_SOFTKEYS_11","(>H:AS3000_PFD_SOFTKEYS_11)"},
	{"SP_AS3000_PFD_SOFTKEYS_12","(>H:AS3000_PFD_SOFTKEYS_12)"},
	{"SP_AS3000_PFD_BottomKnob_Small_INC","(>H:AS3000_PFD_BottomKnob_Small_INC)"},
	{"SP_AS3000_PFD_BottomKnob_Small_DEC","(>H:AS3000_PFD_BottomKnob_Small_DEC)"},
	{"SP_AS3000_PFD_BottomKnob_Push_Long","(>H:AS3000_PFD_BottomKnob_Push_Long)"},
	{"SP_AS3000_PFD_BottomKnob_Push","(>H:AS3000_PFD_BottomKnob_Push)"},
	{"SP_AS3000_PFD_BottomKnob_Large_INC","(>H:AS3000_PFD_BottomKnob_Large_INC)"},
	{"SP_AS3000_PFD_BottomKnob_Large_DEC","(>H:AS3000_PFD_BottomKnob_Large_DEC)"},
	{"SP_AS3000_PFD_TopKnob_Large_INC","(>H:AS3000_PFD_TopKnob_Large_INC)"},
	{"SP_AS3000_PFD_TopKnob_Large_DEC","(>H:AS3000_PFD_TopKnob_Large_DEC)"},
	{"SP_AS3000_PFD_TopKnob_Small_INC","(>H:AS3000_PFD_TopKnob_Small_INC)"},
	{"SP_AS3000_PFD_TopKnob_Small_DEC","(>H:AS3000_PFD_TopKnob_Small_DEC)"},
	{"SP_AS3000_MFD_SOFTKEYS_1","(>H:AS3000_MFD_SOFTKEYS_1)"},
	{"SP_AS3000_MFD_SOFTKEYS_2","(>H:AS3000_MFD_SOFTKEYS_2)"},
	{"SP_AS3000_MFD_SOFTKEYS_3","(>H:AS3000_MFD_SOFTKEYS_3)"},
	{"SP_AS3000_MFD_SOFTKEYS_4","(>H:AS3000_MFD_SOFTKEYS_4)"},
	{"SP_AS3000_MFD_SOFTKEYS_5","(>H:AS3000_MFD_SOFTKEYS_5)"},
	{"SP_AS3000_MFD_SOFTKEYS_6","(>H:AS3000_MFD_SOFTKEYS_6)"},
	{"SP_AS3000_MFD_SOFTKEYS_7","(>H:AS3000_MFD_SOFTKEYS_7)"},
	{"SP_AS3000_MFD_SOFTKEYS_8","(>H:AS3000_MFD_SOFTKEYS_8)"},
	{"SP_AS3000_MFD_SOFTKEYS_9","(>H:AS3000_MFD_SOFTKEYS_9)"},
	{"SP_AS3000_MFD_SOFTKEYS_10","(>H:AS3000_MFD_SOFTKEYS_10)"},
	{"SP_AS3000_MFD_SOFTKEYS_11","(>H:AS3000_MFD_SOFTKEYS_11)"},
	{"SP_AS3000_MFD_SOFTKEYS_12","(>H:AS3000_MFD_SOFTKEYS_12)"},
	{"SP_AS3000_TSC_Horizontal_SoftKey_1","(>H:AS3000_TSC_Horizontal_SoftKey_1)"},
	{"SP_AS3000_TSC_Horizontal_SoftKey_2","(>H:AS3000_TSC_Horizontal_SoftKey_2)"},
	{"SP_AS3000_TSC_Horizontal_SoftKey_3","(>H:AS3000_TSC_Horizontal_SoftKey_3)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Large_INC","(>H:AS3000_TSC_Horizontal_TopKnob_Large_INC)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Large_DEC","(>H:AS3000_TSC_Horizontal_TopKnob_Large_DEC)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Small_INC","(>H:AS3000_TSC_Horizontal_TopKnob_Small_INC)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Small_DEC","(>H:AS3000_TSC_Horizontal_TopKnob_Small_DEC)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Push_Long","(>H:AS3000_TSC_Horizontal_TopKnob_Push_Long)"},
	{"SP_AS3000_TSC_Horizontal_TopKnob_Push","(>H:AS3000_TSC_Horizontal_TopKnob_Push)"},
	{"SP_AS3000_TSC_Horizontal_BottomKnob_Small_INC","(>H:AS3000_TSC_Horizontal_BottomKnob_Small_INC)"},
	{"SP_AS3000_TSC_Horizontal_BottomKnob_Small_DEC","(>H:AS3000_TSC_Horizontal_BottomKnob_Small_DEC)"},
	{"SP_AS3000_TSC_Horizontal_BottomKnob_Push","(>H:AS3000_TSC_Horizontal_BottomKnob_Push)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Small_INC","(>H:AS3000_TSC_Vertical_BottomKnob_Small_INC)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Small_DEC","(>H:AS3000_TSC_Vertical_BottomKnob_Small_DEC)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Push_Long","(>H:AS3000_TSC_Vertical_BottomKnob_Push_Long)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Push","(>H:AS3000_TSC_Vertical_BottomKnob_Push)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Large_INC","(>H:AS3000_TSC_Vertical_BottomKnob_Large_INC)"},
	{"SP_AS3000_TSC_Vertical_BottomKnob_Large_DEC","(>H:AS3000_TSC_Vertical_BottomKnob_Large_DEC)"},
	{"SP_AS3000_TSC_Vertical_TopKnob_Large_INC","(>H:AS3000_TSC_Vertical_TopKnob_Large_INC)"},
	{"SP_AS3000_TSC_Vertical_TopKnob_Large_DEC","(>H:AS3000_TSC_Vertical_TopKnob_Large_DEC)"},
	{"SP_AS3000_TSC_Vertical_TopKnob_Small_INC","(>H:AS3000_TSC_Vertical_TopKnob_Small_INC)"},
	{"SP_AS3000_TSC_Vertical_TopKnob_Small_DEC","(>H:AS3000_TSC_Vertical_TopKnob_Small_DEC)"},
	{"SP_KAP140_Push_AP","(>H:KAP140_Push_AP)"},
	{"SP_KAP140_Push_HDG","(>H:KAP140_Push_HDG)"},
	{"SP_KAP140_Push_NAV","(>H:KAP140_Push_NAV)"},
	{"SP_KAP140_Push_APR","(>H:KAP140_Push_APR)"},
	{"SP_KAP140_Push_REV","(>H:KAP140_Push_REV)"},
	{"SP_KAP140_Push_ALT","(>H:KAP140_Push_ALT)"},
	{"SP_KAP140_Push_UP","(>H:KAP140_Push_UP)"},
	{"SP_KAP140_Push_DN","(>H:KAP140_Push_DN)"},
	{"SP_KAP140_Long_Push_BARO","(>H:KAP140_Long_Push_BARO)"},
	{"SP_KAP140_Push_BARO","(>H:KAP140_Push_BARO)"},
	{"SP_KAP140_Push_ARM","(>H:KAP140_Push_ARM)"},
	{"SP_KAP140_Knob_Inner_INC","(>H:KAP140_Knob_Inner_INC)"},
	{"SP_KAP140_Knob_Inner_DEC","(>H:KAP140_Knob_Inner_DEC)"},
	{"SP_KAP140_Knob_Outer_INC","(>H:KAP140_Knob_Outer_INC)"},
	{"SP_KAP140_Knob_Outer_DEC","(>H:KAP140_Knob_Outer_DEC)"},
	{"SP_oclock_select","(>H:oclock_select)"},
	{"SP_oclock_oat","(>H:oclock_oat)"},
	{"SP_oclock_control_long","(>H:oclock_control_long)"},
	{"SP_oclock_control","(>H:oclock_control)"},
	{"SP_AS530_ENT_Push","(>H:AS530_ENT_Push)"},
	{"SP_AS530_MENU_Push","(>H:AS530_MENU_Push)"},
	{"SP_AS530_FPL_Push","(>H:AS530_FPL_Push)"},
	{"SP_AS530_DirectTo_Push","(>H:AS530_DirectTo_Push)"},
	{"SP_AS530_CLR_Push_Long","(>H:AS530_CLR_Push_Long)"},
	{"SP_AS530_CLR_Push","(>H:AS530_CLR_Push)"},
	{"SP_AS530_MSG_Push","(>H:AS530_MSG_Push)"},
	{"SP_AS530_OBS_Push","(>H:AS530_OBS_Push)"},
	{"SP_AS530_VNAV_Push","(>H:AS530_VNAV_Push)"},
	{"SP_AS530_PROC_Push","(>H:AS530_PROC_Push)"},
	{"SP_AS530_COMSWAP_Push","(>H:AS530_COMSWAP_Push)"},
	{"SP_AS530_NAVSWAP_Push","(>H:AS530_NAVSWAP_Push)"},
	{"SP_AS530_RNG_Dezoom","(>H:AS530_RNG_Dezoom)"},
	{"SP_AS530_RNG_Zoom","(>H:AS530_RNG_Zoom)"},
	{"SP_AS530_RightLargeKnob_Right","(>H:AS530_RightLargeKnob_Right)"},
	{"SP_AS530_RightLargeKnob_Left","(>H:AS530_RightLargeKnob_Left)"},
	{"SP_AS530_LeftLargeKnob_Right","(>H:AS530_LeftLargeKnob_Right)"},
	{"SP_AS530_LeftLargeKnob_Left","(>H:AS530_LeftLargeKnob_Left)"},
	{"SP_AS530_RightSmallKnob_Right","(>H:AS530_RightSmallKnob_Right)"},
	{"SP_AS530_RightSmallKnob_Left","(>H:AS530_RightSmallKnob_Left)"},
	{"SP_AS530_RightSmallKnob_Push","(>H:AS530_RightSmallKnob_Push)"},
	{"SP_AS530_LeftSmallKnob_Right","(>H:AS530_LeftSmallKnob_Right)"},
	{"SP_AS530_LeftSmallKnob_Left","(>H:AS530_LeftSmallKnob_Left)"},
	{"SP_AS530_LeftSmallKnob_Push","(>H:AS530_LeftSmallKnob_Push)"},
	{"SP_AS430_ENT_Push","(>H:AS430_ENT_Push)"},
	{"SP_AS430_MENU_Push","(>H:AS430_MENU_Push)"},
	{"SP_AS430_FPL_Push","(>H:AS430_FPL_Push)"},
	{"SP_AS430_DirectTo_Push","(>H:AS430_DirectTo_Push)"},
	{"SP_AS430_CLR_Push_Long","(>H:AS430_CLR_Push_Long)"},
	{"SP_AS430_CLR_Push","(>H:AS430_CLR_Push)"},
	{"SP_AS430_MSG_Push","(>H:AS430_MSG_Push)"},
	{"SP_AS430_OBS_Push","(>H:AS430_OBS_Push)"},
	{"SP_AS430_PROC_Push","(>H:AS430_PROC_Push)"},
	{"SP_AS430_COMSWAP_Push","(>H:AS430_COMSWAP_Push)"},
	{"SP_AS430_NAVSWAP_Push","(>H:AS430_NAVSWAP_Push)"},
	{"SP_AS430_RNG_Dezoom","(>H:AS430_RNG_Dezoom)"},
	{"SP_AS430_RNG_Zoom","(>H:AS430_RNG_Zoom)"},
	{"SP_AS430_RightLargeKnob_Right","(>H:AS430_RightLargeKnob_Right)"},
	{"SP_AS430_RightLargeKnob_Left","(>H:AS430_RightLargeKnob_Left)"},
	{"SP_AS430_LeftLargeKnob_Right","(>H:AS430_LeftLargeKnob_Right)"},
	{"SP_AS430_LeftLargeKnob_Left","(>H:AS430_LeftLargeKnob_Left)"},
	{"SP_AS430_RightSmallKnob_Right","(>H:AS430_RightSmallKnob_Right)"},
	{"SP_AS430_RightSmallKnob_Left","(>H:AS430_RightSmallKnob_Left)"},
	{"SP_AS430_RightSmallKnob_Push","(>H:AS430_RightSmallKnob_Push)"},
	{"SP_AS430_LeftSmallKnob_Right","(>H:AS430_LeftSmallKnob_Right)"},
	{"SP_AS430_LeftSmallKnob_Left","(>H:AS430_LeftSmallKnob_Left)"},
	{"SP_AS430_LeftSmallKnob_Push","(>H:AS430_LeftSmallKnob_Push)"},
	{"SP_adf_AntAdf","(>H:adf_AntAdf)"},
	{"SP_adf_bfo","(>H:adf_bfo)"},
	{"SP_adf_frqTransfert","(>H:adf_frqTransfert)"},
	{"SP_adf_FltEt","(>H:adf_FltEt)"},
	{"SP_adf_SetRst","(>H:adf_SetRst)"},
	{"SP_TransponderIDT","(>H:TransponderIDT)"},
	{"SP_TransponderVFR","(>H:TransponderVFR)"},
	{"SP_TransponderCLR","(>H:TransponderCLR)"},
	{"SP_Transponder0","(>H:Transponder0)"},
	{"SP_Transponder1","(>H:Transponder1)"},
	{"SP_Transponder2","(>H:Transponder2)"},
	{"SP_Transponder3","(>H:Transponder3)"},
	{"SP_Transponder4","(>H:Transponder4)"},
	{"SP_Transponder5","(>H:Transponder5)"},
	{"SP_Transponder6","(>H:Transponder6)"},
	{"SP_Transponder7","(>H:Transponder7)"}

};

void CALLBACK MyDispatchProc1(SIMCONNECT_RECV* pData, DWORD cbData, void* pContext);

extern "C" MSFS_CALLBACK void module_init(void)
{
	int i;

	printf("SimPanel: Connecting to SimConnect...\r\n");

	g_hSimConnect = 0;


	if (SUCCEEDED(SimConnect_Open(&g_hSimConnect, "SimPanel Module", nullptr, 0, 0, 0)))
	{
		printf("SimPanel: SimConnect connected\r\n");

		for (i = 0; i < 272; i++)
		{
			SimConnect_MapClientEventToSimEvent(g_hSimConnect, i, events[i][0].c_str());
			SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, GROUP0, i);
		}

		printf("SimPanel: Registration complete\r\n");


		if (SUCCEEDED(SimConnect_SetNotificationGroupPriority(g_hSimConnect, GROUP0, SIMCONNECT_GROUP_PRIORITY_DEFAULT))) {

			printf("SimPanel: Group Priority succeded\r\n");
			if(SUCCEEDED(SimConnect_CallDispatch(g_hSimConnect, MyDispatchProc1, NULL)))
			{
				printf("SimPanel: CallDispatch succeded\r\n");
			}
		}
	}


	//hr = SimConnect_MapClientEventToSimEvent(g_hSimConnect, SIMPANEL_FLAPS_DECR, "SIMPANEL.FLAPS_DECR");
	//hr = SimConnect_MapClientEventToSimEvent(g_hSimConnect, SIMPANEL_FLAPS_INCR, "SIMPANEL.FLAPS_INCR");
	//hr = SimConnect_MapClientEventToSimEvent(g_hSimConnect, KEY_FLAPS_INCR, "FLAPS_INCR");
	//hr = SimConnect_MapClientEventToSimEvent(g_hSimConnect, KEY_FLAPS_DECR, "FLAPS_DECR");

	//hr = SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, GROUP0, SIMPANEL_FLAPS_DECR);
	//hr = SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, GROUP0, SIMPANEL_FLAPS_INCR);
	//hr = SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, GROUP0, KEY_FLAPS_DECR);
	//hr = SimConnect_AddClientEventToNotificationGroup(g_hSimConnect, GROUP0, KEY_FLAPS_INCR);
}

extern "C" MSFS_CALLBACK void module_deinit(void)
{

	if (!g_hSimConnect)
		return;
	HRESULT hr = SimConnect_Close(g_hSimConnect);
	if (hr != S_OK)
	{
		fprintf(stdout, "Could not close SimConnect connection.\n");
		return;
	}

}


void CALLBACK MyDispatchProc1(SIMCONNECT_RECV* pData, DWORD cbData, void* pContext)
{
	switch (pData->dwID)
	{
	case SIMCONNECT_RECV_ID_EVENT:
	{
		SIMCONNECT_RECV_EVENT* evt = (SIMCONNECT_RECV_EVENT*)pData;
		if (evt->uEventID <= 272)
		{
			printf("SimPanel: ReceivedEvent\r\n");
			execute_calculator_code(events[evt->uEventID][1].c_str(), 0, 0, 0);

		}
		//switch (evt->uEventID)
		//{
		//case SIMPANEL_FLAPS_DECR:
		//	SimConnect_TransmitClientEvent(g_hSimConnect, SIMCONNECT_OBJECT_ID_USER, KEY_FLAPS_DECR, 1, GROUP0, SIMCONNECT_EVENT_FLAG_GROUPID_IS_PRIORITY);

		//	execute_calculator_code("(>H:Transponder5)", 0, 0, 0);
		//	printf("\nEvent SIMPANEL_FLAPS_DECR: %d", evt->dwData);
		//	break;

		//case SIMPANEL_FLAPS_INCR:
		//	SimConnect_TransmitClientEvent(g_hSimConnect, SIMCONNECT_OBJECT_ID_USER, KEY_FLAPS_INCR, 1, GROUP0, SIMCONNECT_EVENT_FLAG_GROUPID_IS_PRIORITY);
		//	execute_calculator_code("(>H:Transponder4)", 0, 0, 0);
		//	printf("\nEvent SIMPANEL_FLAPS_INCR: %d", evt->dwData);

		//	break;

		//default:
		//	break;
		//}
		break;
	}



	}
}


//void CALLBACK MyDispatchProc(SIMCONNECT_RECV* pData, DWORD cbData, void* pContext)
//{
//	fprintf(stdout, "BELEPTUNK A MY DISPATCH PROCba\n");
//	switch (pData->dwID)
//	{
//		case SIMCONNECT_RECV_ID_EVENT_FILENAME:
//		{
//			SIMCONNECT_RECV_EVENT_FILENAME* evt = (SIMCONNECT_RECV_EVENT_FILENAME*)pData;
//			switch (evt->uEventID)
//			{
//			case EVENT_FLIGHT_LOADED:
//
//				fprintf(stdout, "New Flight Loaded: %s\n", evt->szFileName);
//				break;
//			default:
//				break;
//			}
//			break;
//		};
//
//		case SIMCONNECT_RECV_ID_EVENT:
//		{
//			SIMCONNECT_RECV_EVENT* evt = (SIMCONNECT_RECV_EVENT*)pData;
//
//			fprintf(stdout, "JOTT EGY RECV ESEMENY\n");
//			switch (evt->uEventID)
//			{
//			case SIMPANEL_FLAPS_DECR:
//				// SIMPANEL_FLAPS_DECR, 1, SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
//
//				//this.SimConnect.TransmitClientEvent(0, ev, 1, SIMCONNECT_NOTIFICATION_GROUP_ID.SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
//
//				//this.SimConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, ev, data, SimEnum.group1, Microsoft.FlightSimulator.SimConnect.SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
//				SimConnect_TransmitClientEvent(g_hSimConnect, 0, KEY_FLAPS_DECR, 1, SIMCONNECT_GROUP_PRIORITY_DEFAULT, SIMCONNECT_EVENT_FLAG_GROUPID_IS_PRIORITY);
//				SimConnect_TransmitClientEvent(g_hSimConnect, SIMCONNECT_OBJECT_ID_USER, KEY_FLAPS_DECR, 0, 0, SIMCONNECT_EVENT_FLAG_GROUPID_IS_PRIORITY);
//
//				fprintf(stdout, "Fired: SIMPANEL_FLAPS_DECR \n");
//				break;
//
//			case SIMPANEL_FLAPS_INCR:
//				fprintf(stdout, "Fired: SIMPANEL_FLAPS_INC \n");
//				break;
//
//			default:
//				break;
//			}
//		};
//	default:
//		break;
//	}
//}
