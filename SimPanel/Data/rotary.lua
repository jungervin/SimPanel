-- Fölső: D1:R3=1 
-- Közép: D1:R2=1 
-- Alsó : D1:R1=1 
function Process(trigger)
    if trigger == "D1:R3=1" then
        flc = GetVariableValue("AUTOPILOT FLIGHT LEVEL CHANGE");
        vs = GetVariableValue("AUTOPILOT VERTICAL HOLD");

        if flc == 1 then
            return SendEvent("AP_SPD_VAR_INC", 0)
        elseif vs == 1 then
            return SendEvent("AP_VS_VAR_INC", 0)
        end

    elseif trigger == "D1:R3=-1" then

        flc = GetVariableValue("AUTOPILOT FLIGHT LEVEL CHANGE");
        vs = GetVariableValue("AUTOPILOT VERTICAL HOLD");
        if flc == 1 then
            return SendEvent("AP_SPD_VAR_DEC", 0)
        elseif vs == 1 then
            return SendEvent("AP_VS_VAR_DEC", 0)
        end
        
    elseif trigger == "D1:R3=10" then

        flc = GetVariableValue("AUTOPILOT FLIGHT LEVEL CHANGE");
        vs = GetVariableValue("AUTOPILOT VERTICAL HOLD");
        if flc == 1 then
            SendEvent("AP_SPD_VAR_INC", 0)
            SendEvent("AP_SPD_VAR_INC", 0)
        elseif vs == 1 then
            SendEvent("AP_VS_VAR_INC", 0)
            SendEvent("AP_VS_VAR_DEC", 0)
        end

    elseif trigger == "D1:R3=-10" then

        flc = GetVariableValue("AUTOPILOT FLIGHT LEVEL CHANGE");
        vs = GetVariableValue("AUTOPILOT VERTICAL HOLD");
        if flc == 1 then
            SendEvent("AP_SPD_VAR_DEC", 0)
            SendEvent("AP_SPD_VAR_DEC", 0)
        elseif vs == 1 then
            SendEvent("AP_VS_VAR_DEC", 0)
            SendEvent("AP_VS_VAR_DEC", 0)
        end

    elseif trigger == "D1:R2=1" then
        SendEvent("HEADING_BUG_INC", 0)

    elseif trigger == "D1:R2=10" then
        SendEvent("HEADING_BUG_INC", 0)
        SendEvent("HEADING_BUG_INC", 0)

    elseif trigger == "D1:R2=-1" then
        SendEvent("HEADING_BUG_DEC", 0)

    elseif trigger == "D1:R2=-10" then
        SendEvent("HEADING_BUG_DEC", 0)
        SendEvent("HEADING_BUG_DEC", 0)

    elseif trigger == "D1:R1=1" then
        SendEvent("AP_ALT_VAR_INC", 0)

    elseif trigger == "D1:R1=01" then
        SendEvent("AP_ALT_VAR_INC", 0)
        SendEvent("AP_ALT_VAR_INC", 0)

    elseif trigger == "D1:R1=-1" then
        SendEvent("AP_ALT_VAR_DEC", 0)

    elseif trigger == "D1:R1=-10" then
        SendEvent("AP_ALT_VAR_DEC", 0)
        SendEvent("AP_ALT_VAR_DEC", 0)
    end

    return trigger
end

function PreProcess2(trigger)
    flc = GetVariableValue("AUTOPILOT FLIGHT LEVEL CHANGE");
    vs = GetVariableValue("AUTOPILOT VERTICAL HOLD");
    if flc == 1 then
        if trigger == "D1:R3=1" then
            return "AP_SPD_VAR_INC"
        else
            return "AP_SPD_VAR_DEC"
        end
    elseif vs == 1 then
        if trigger == "D1:R3=1" then
            return "AP_VS_VAR_INC"
        else
            return "AP_VS_VAR_DEC"
        end
    end
    return "NULL"
end
