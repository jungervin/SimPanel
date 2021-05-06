#define HW_ID "H1"

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.setTimeout(500);
  Serial.println("SWITCH PLATE");
  Serial.print("HARDWARE ID: H1");
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);
  pinMode(5, INPUT_PULLUP);
  pinMode(6, INPUT_PULLUP);
  pinMode(7, INPUT_PULLUP);
  pinMode(8, INPUT_PULLUP);
  pinMode(9, INPUT_PULLUP);
  pinMode(10, INPUT_PULLUP);
  pinMode(11, INPUT_PULLUP);
  pinMode(12, INPUT_PULLUP);
  pinMode(13, INPUT_PULLUP);
  
}


String received = "";

void printStates() {

  Serial.print("H1D2:");
  Serial.println(digitalRead(2));

  Serial.print("H1D3:");
  Serial.println(digitalRead(3));

  Serial.print("H1D4:");
  Serial.println(digitalRead(4));

  Serial.print("H1D5:");
  Serial.println(digitalRead(5));
  
  Serial.print("H1D6:");
  Serial.println(digitalRead(6));
  
  Serial.print("H1D7:");
  Serial.println(digitalRead(7));
  
  Serial.print("H1D8:");
  Serial.println(digitalRead(8));

  Serial.print("H1D9:");
  Serial.println(digitalRead(9));

  Serial.print("H1D10:");
  Serial.println(digitalRead(10));
  
  Serial.print("H1D11:");
  Serial.println(digitalRead(11));
  
  Serial.print("H1D12:");
  Serial.println(digitalRead(12));

  Serial.print("H1D13:");
  Serial.println(digitalRead(13));

  Serial.println('\04');
  
}

void processQuery(String q) {
  if(q == "getHW") {
    Serial.println("HW1");
  }
  else if(q == "getStates") {
    printStates();
  }
  else {
    Serial.println("UNKNOWN")  ;
  }
}

void loop() {
  // put your main code here, to run repeatedly:

//  while (Serial.available()) {
//   
//    char c = Serial.read();
//    
//    if(received.length() > 16) {
//      received = "";
//    }
//
//    if(c == '\n')
//    {
//      processQuery(received);
//      received = "";  
//    }
//    else if(c >= ' ') {
//      received += c;
//    }
//  }

  printStates();
  
  delay(500);
 
}
