









//We always have to include the library
//include "LedControl.h"

#include "EncoderStepCounter.h"
#define ENCODER1_PIN1 2
#define ENCODER1_PIN2 3
#define ENCODER3_PIN1 4
#define ENCODER3_PIN2 5
#define ENCODER2_PIN1 6
#define ENCODER2_PIN2 7


/*
           Create a new controler
           Params :
           dataPin    pin on the Arduino where data gets shifted out
           clockPin   pin for the clock
           csPin    pin for selecting the device
           numDevices maximum number of devices that can be controled
           LedControl(int dataPin, int clkPin, int csPin, int numDevices=1);
*/
// kék CLK D10 13
// zöld CS D11 14
// sárga DIN D12 15

//>#define dataPin 15
//#define csPin 11
//#define clkPin 13
//
//LedControl lc=LedControl(12,10,11,2);
////LedControl lc=LedControl(dataPin, clkPin, csPin, 2);
EncoderStepCounter encoder1(ENCODER1_PIN1, ENCODER1_PIN2, FULL_STEP);
EncoderStepCounter encoder2(ENCODER2_PIN1, ENCODER2_PIN2, FULL_STEP);
EncoderStepCounter encoder3(ENCODER3_PIN1, ENCODER3_PIN2, FULL_STEP);
#define MIN 0
#define MAX 360

unsigned long delaytime = 1000;
String rec = "";
void setup() {
  Serial.begin(9600);
  //Serial.println("STARTED");
  encoder1.begin();
  encoder2.begin();
  encoder3.begin();
}


signed int lastpos = 0;
signed long position1 = 0;
signed long position2 = 0;
unsigned long e1t1;
unsigned long e1t2;
unsigned long e2t1;
unsigned long e2t2;
unsigned long e3t1;
unsigned long e3t2;


#define speed 50
void loop() {

  signed char pos = encoder1.getPosition();
  if (pos != 0) {

    e1t2 = millis();
    if (e1t2 - e1t1 < speed)
    {
      pos = pos * 10;
    }

    Serial.print("D1:R1=");
    Serial.println(pos, DEC);

    encoder1.reset();
    e1t1 = e1t2;
  }
  encoder1.tick();


  pos = encoder2.getPosition();
  if (pos != 0) {

    e2t2 = millis();
    if (e2t2 - e2t1 < speed)
    {
      pos = pos * 10;
    }

    Serial.print("D1:R2=");
    Serial.println(pos, DEC);

    encoder2.reset();
    e2t1 = e2t2;
  }
  encoder2.tick();


  pos = encoder3.getPosition();
  if (pos != 0) {

    e3t2 = millis();
    if (e3t2 - e3t1 < speed)
    {
      pos = pos * 10;
    }

    Serial.print("D1:R3=");
    Serial.println(pos, DEC);

    encoder3.reset();
    e3t1 = e3t2;
  }
  encoder3.tick();


}
