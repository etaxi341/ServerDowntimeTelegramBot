# ServerDowntimeTelegramBot

## What is this?

This is a little application that will notify you via Telegram whenever the targetserver is not reachable anymore.
It will check for:

- HTTP/HTTPS sites
- pingable IPs
- open MSQL Server Port

## How to use

1. Create a Telegram bot by messaging @BotFather on Telegram
2. Allow the Bot to text you by messaging your created bot on Telegram
3. Get your Chat_Id by messaging @userinfobot on Telegram
4. Create a schedule in the Windows scheduler to run it every minute

These are the parameters you need to run the Bot
- PATH_TO_PROGRAM.EXE
- Bot Token from your created Bot (@BotFather tells you your Token)
- Any Server Name you want to give it
- The IP Address you want to check
- An infinite Amount of Chat_Ids you got from @userinfobot

### Example

    ServerDowntimeTelegramBot.exe "1022838193:AXE-p8-u624Tv3NxYHRsTVDz614s2klcvsf" "Google Germany" "216.58.207.67" "514300732" "592310342"

This would try to ping the German Google Servers and send a Message to "514300732" and "592310342" when they are down.
