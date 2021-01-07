# import obd
#
# connection = obd.OBD() # auto-connects to USB or RF port
#
# cmd = obd.commands.SPEED # select an OBD command (sensor)
#
# while True:
#
#     response = connection.query(cmd)  # send the command, and parse the response
#
#
#
#     print(response.value)  # returns unit-bearing values thanks to Pint
#     print(response.value.to("mph"))  # user-friendly unit conversions


#!/usr/bin/python

# Import the appropriate modules
import obd
from time import sleep
import pygame
from time import strftime

# Set the screen size
size = (600,300)

# Initialise pygame
pygame.init()






obd.logger.setLevel(obd.logging.DEBUG)

connection = obd.OBD(baudrate=38400, portstr= '/dev/tty.usbserial')


#response = connection.query(obd.commands.RPM)


# Set our screen size
screen = pygame.display.set_mode(size)

# Set header (useful for testing, not so much for full screen mode!)
pygame.display.set_caption("Test clock script")

# Stop keys repeating (not so necessary for this script, but useful if you want to capture other key presses)
pygame.key.set_repeat()


# Function to show our clock
def showClock(clockScreen, connection):
    # Fill the screen with a black background
    clockScreen.fill((0,0,0))

    # Define some fonts to draw text with
    myfont = pygame.font.SysFont(None, 100)
    myfontsmall = pygame.font.SysFont(None, 50)

    response = connection.query(obd.commands.SPEED)  # send the command, and parse the response

    # Create the strings to display
    # mytime = strftime("%H:%M")
    mytime = str(response.value)
    mysecs = strftime("%S")



   # Render the strings
    clocklabel = myfont.render(mytime, 1, (255,255,255))
    secondlabel = myfontsmall.render(mysecs, 1, (255,255,255))

    # And position them on the screen
    textpos = clocklabel.get_rect() # Gets the rectangle of the hours and minutes...
    textpos.centerx = clockScreen.get_rect().centerx # ...and center horizontally...
    textpos.centery = clockScreen.get_rect().centery # ...and vertically
    secpos = (textpos[0] + textpos[2] + 10, textpos[1] + textpos[3] - 55) # A bit of trial and error to position the seconds

     # Draw the text onto our screen
    # clockScreen.blit(secondlabel, secpos)
    clockScreen.blit(clocklabel, textpos)

    # Update the display (i.e. show the output of the above!)
    pygame.display.flip()


# Set up a boolean for a clean loop
quitloop=False

# Set up a variable to check when to refresh display
refresh = 0

# Run our main loop
while not quitloop:
    for event in pygame.event.get():

        # Handle quit message received
        if event.type == pygame.QUIT:
           quitloop = True

        # 'Q' to quit    
        if (event.type == pygame.KEYUP):
           if (event.key == pygame.K_q):
               quitloop = True

    # If pygame's clock is greater than our variable then we need to update display
    if pygame.time.get_ticks() > refresh:

        # Run the function to update display      
        showClock(screen, connection)

        # Update refresh time to 500ms in the future
        refresh = pygame.time.get_ticks()





