void setup() {
  Serial.begin(9600);
  Serial.setTimeout(10000);
}

int serialInt;

int instructions[100][2];
int instructionsCount;

void loop() {
  Serial.print("ready\n");
  delay(100);
  if (Serial.available() > 0)
  {
    readInstructions();
    execute();
  }
}

void readInstructions()
{
  int loop;
  
  instructionsCount = Serial.parseInt();
  for (loop = 0; loop < instructionsCount; loop++)
  {
    instructions[loop][0] = Serial.parseInt();
    instructions[loop][1] = Serial.parseInt();
  }  
  
  Serial.print("received ");
  Serial.print(instructionsCount);
  Serial.print(" instructions\n");
}

void execute()
{
  Serial.print("executing\n");
  int loop;

  for (loop = 0; loop < instructionsCount; loop++)
  {
    var opCode = instructions[loop][0];
    var param = instructions[loop][1];

    switch(opCode)
    {
      case 1: delay(param); break;
      case 2: delayMicroseconds(param); break;
      case 3: pinMode(param, OUTPUT); break;
      case 4: digitalWrite(param, HIGH); break;
      case 5: digitalWrite(param, LOW); break;      
    }
  }
  
  Serial.print("done\n");
}



