#define powerSwitch 13
#define powerButton 4
#define tempPin 6
#define smokeDPin 2
#define smokeAPin A0
#define DS18S20_ID 0x10
#define DS18B20_ID 0x28
#include "CurieIMU.h"
#include <OneWire.h>
OneWire ds18b20(tempPin);

boolean power = false;
float temp;
byte data[12];
byte addr[8];
int tempReadDelay = 1000;
long lastTempRead;
boolean read_get = true;
boolean Connected = false;
int connectedDelay = 20000;

boolean motion = false;
boolean smoke = false;
int smokeResetDelay = 30000;
long lastSmokeConfirmed;

boolean readTemperature()
{
  if (!ds18b20.search(addr)) {
    ds18b20.reset_search();
    return false;
  }
  if (OneWire::crc8( addr, 7) != addr[7]) {
    return false;
  }
  if (addr[0] != DS18S20_ID && addr[0] != DS18B20_ID) {
    return false;
  }

  ds18b20.reset();
  ds18b20.select(addr);
  // Start conversion
  ds18b20.write(0x44, 1);
  // Wait some time...
}

boolean getTemperature() {
  byte i;
  byte present = 0;
  present = ds18b20.reset();
  ds18b20.select(addr);
  // Issue Read scratchpad command
  ds18b20.write(0xBE);
  // Receive 9 bytes
  for ( i = 0; i < 9; i++) {
    data[i] = ds18b20.read();
  }
  // Calculate temperature value
  temp = (( (data[1] << 8) + data[0] ) * 0.0625);
  return true;

}

void setup() {
  // initialize the digital pin as an output.
  Serial.begin(9600);
  pinMode(powerSwitch, OUTPUT);
  pinMode(powerButton, INPUT);
  pinMode(smokeDPin, INPUT);
  pinMode(smokeAPin, INPUT);
  //pinMode(tempPin, INPUT);
  digitalWrite(powerButton, HIGH);
  digitalWrite(powerSwitch, LOW);
  readTemperature();
  delay(1000);
  getTemperature();
  
  CurieIMU.begin();
  CurieIMU.attachInterrupt(eventCallback);

  /* Enable Zero Motion Detection */
  CurieIMU.setDetectionThreshold(CURIE_IMU_ZERO_MOTION, 50);  // 50mg
  CurieIMU.setDetectionDuration(CURIE_IMU_ZERO_MOTION, 120);    // 2s
  CurieIMU.interrupts(CURIE_IMU_ZERO_MOTION);

  /* Enable Motion Detection */
  CurieIMU.setDetectionThreshold(CURIE_IMU_MOTION, 20);      // 20mg
  CurieIMU.setDetectionDuration(CURIE_IMU_MOTION, 10);       // trigger times of consecutive slope data points
  CurieIMU.interrupts(CURIE_IMU_MOTION);
}

// the loop routine runs over and over again forever:
void loop() {

  //turns led on and off based on sending 0 or 1 from serial terminal
  if (Serial.available()) {
    char input = Serial.read();
    if (input == '0') {
      power = false;
    }
    else if (input == '1') {
      power = true;
    }
    else if (input == 'i')
    {
      if (power)
        Serial.println('1');
      else
        Serial.println('0');
    }
    else if (input == 't')
    {
      String output = "T=";
      output += temp;
      Serial.println(output);
    }
    else if (input == 'm')
    {
      String output = "M=";
      output += motion;
      Serial.println(output);
    }
    else if (input == 's')
    {
      String output = "S=";
      output += smoke;
      Serial.println(output);
    }
  }
  if (digitalRead(powerButton) == LOW) {
    power = !power;
    delay(250);
  }
  if (power)
    digitalWrite(powerSwitch, 255);
  else
    digitalWrite(powerSwitch, 0);
  if (digitalRead (smokeDPin) == LOW){
    smoke = true;
    lastSmokeConfirmed = millis();
  }
  if (analogRead(smokeAPin) > 150){
     smoke = true;
    lastSmokeConfirmed = millis();
  }
  //Serial.print("Smoke=");
 // Serial.print(analogRead(smokeAPin));
 // Serial.print("  SmokeD=");
 // Serial.println(digitalRead(smokeDPin));
  if ((millis() - lastTempRead) > tempReadDelay ) {
    if (read_get)
      readTemperature();
    else
      getTemperature();
    read_get = !read_get;
    lastTempRead = millis();
  }
  if ((smoke == true) && (lastSmokeConfirmed > smokeResetDelay))
    smoke = false;
}

static void eventCallback(void){
  if (CurieIMU.getInterruptStatus(CURIE_IMU_ZERO_MOTION)) {
    motion = false; 
   // Serial.println("zero motion detected...");
  }  
  if (CurieIMU.getInterruptStatus(CURIE_IMU_MOTION)) {
    motion = true;

  }  
}
