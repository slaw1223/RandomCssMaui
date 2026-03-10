// piny segmentów (a,b,c,d,e,f,g)
int segPins[] = {2,3,4,5,6,7,9};

byte numbers[10][7] = {
  {1,1,1,1,1,1,0}, //0
  {0,1,1,0,0,0,0}, //1
  {1,1,0,1,1,0,1}, //2
  {1,1,1,1,0,0,1}, //3
  {0,1,1,0,0,1,1}, //4
  {1,0,1,1,0,1,1}, //5
  {1,0,1,1,1,1,1}, //6
  {1,1,1,0,0,0,0}, //7
  {1,1,1,1,1,1,1}, //8
  {1,1,1,1,0,1,1}  //9
};

int dataPin = 11;
int clockPin = 12;
int latchPin = 8;

void setup() {
  for(int i=0;i<7;i++) pinMode(segPins[i], OUTPUT);
  
  pinMode(dataPin, OUTPUT);
  pinMode(clockPin, OUTPUT);
  pinMode(latchPin, OUTPUT);

  
  Serial.begin(9600);
}


void displayDigit(int num){
  for(int i=0;i<7;i++){
    digitalWrite(segPins[i], numbers[num][i] ? LOW : HIGH);
  }
}

void animateLEDs(){
  for(int j=0;j<3;j++){
    for(int i=0;i<8;i++){
      digitalWrite(latchPin, LOW);
      byte value = 1 << i;
      shiftOut(dataPin, clockPin, MSBFIRST, value);
      digitalWrite(latchPin,HIGH);
      delay(100);
    }
  }
}

void loop() {
  if(Serial.available()){
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();

    if(cmd.startsWith("START_ANIM | ")){
      animateLEDs();

      int separatorIndex = cmd.indexOf('|');
      if(separatorIndex > 0 && separatorIndex + 2 < cmd.length()){
        String idStr = cmd.substring(separatorIndex + 2);
        int studentId = idStr.toInt();
        if(studentId >= 0 && studentId <= 9){
          displayDigit(studentId);
        }
      }
    }
  }
}