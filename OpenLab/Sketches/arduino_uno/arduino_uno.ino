String Buffer = "";
short Mode;
short Pin;

enum
{
  ANALOG,
  DIGITAL
} PinType;

void setup()
{
  Serial.begin(115200);
}

void loop()
{
  while(Serial.available())
  {
    Buffer += (char)Serial.read();

    if (Buffer.endsWith("\n"))
    { 
      if (Buffer.startsWith("DIGITAL"))
      {
        PinType = DIGITAL;
      }
      else if (Buffer.startsWith("ANALOG"))
      {
        PinType = ANALOG;
      }
      
      Buffer = Buffer.substring(Buffer.indexOf("_") + 1);
      
      if (Buffer.startsWith("INPUT"))
      {
        Mode = INPUT;
      }
      else if (Buffer.startsWith("OUTPUT"))
      {
        Mode = OUTPUT;
      }
      else if (Buffer.startsWith("INPUT_PULLUP"))
      {
        Mode = INPUT_PULLUP; 
      }

      Buffer = Buffer.substring(Buffer.indexOf(" ") + 1);
      
      if (Buffer.startsWith("A"))
      {
        Buffer = Buffer.substring(1);
      }
    
      Pin = Buffer.toInt();
      Buffer = Buffer.substring(Buffer.indexOf(" ") + 1);

      if (Buffer.startsWith("INIT"))
      {
        pinMode(Pin, Mode);
      }
      else if (Buffer.startsWith("READ"))
      {
        if (PinType == ANALOG)
        {
          Serial.println(analogRead(Pin));
        }
        else
        {
          Serial.println(digitalRead(Pin));
        }
      }
      else if (Buffer.startsWith("WRITE"))
      {
        Buffer = Buffer.substring(Buffer.indexOf(" ") + 1);
        int value = Buffer.toInt();
        
        if (PinType == ANALOG)
        {
          analogWrite(Pin, value);
        }
        else
        {
          digitalWrite(Pin, value);
        }
      }

      Buffer = "";
    }
  }
}
