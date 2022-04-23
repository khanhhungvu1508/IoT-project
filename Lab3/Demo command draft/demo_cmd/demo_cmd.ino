float randNumber;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  randomSeed(analogRead(0));
}

void loop() {
  // put your main code here, to run repeatedly:
  randNumber = random(0, 100);
  Serial.print("!TEMP:" + String(randNumber, 1) + "#");
  randNumber = random(0, 100);
  Serial.print("!LIGHT:" + String(randNumber, 1) + "#");
//  while (Serial.available() > 0) {
//    char cv = Serial.read();
//    Serial.print(cv);
//  }
  delay(1000);
}
