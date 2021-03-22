Hands-on task Greentube 
 
You are asked to implement a new .Net Core web service that allows players to log in case they 
forgot their current password simply by knowing the email address they used at registration 
(which is unique for each player). The service will run locally (on the dev machine) so there’s no 
need to deploy anything. 
 
Those are the specific requirements: 
 
1.  Has 2 endpoints:  
a.  "forgot password" endpoint accepts as input an email address and sends an email to 
that address. The email contains the following: 
 
"Dear player, 
In order to log in to your account so you can change your password click the 
following link: 
 
<URL here pointing to the second endpoint described below> 
 
Note that this link is only valid for 1 hour, after which it expires!  
 
Happy playing! 
Greentube Support Team" 
 
(you don't need to implement the actual sending of the email; you can simply write 
this to a file and call that "sending of the email"). 
 
b.  "temp login" endpoint responds to players accessing the link sent in the email above.  
 
You need to verify that the link has not expired (i.e. was produced less than 1 hour 
ago). If link still valid, proceed to login the player having the email address that the 
link was produced for (you don't need to actually do  anything here, as this would 
require DB access, etc - just call a dummy method called "LoginByEmailAddress" 
which takes the email address of the account as a parameter).  
 
 
2.  Implement some unit tests. Don't need to cover everything, just a few the methods (start with 
those that you consider are important to cover). 
 
3.  If time permits: what possible vulnerabilities to you see in this service (i.e. do you see any 
potential of abuse by callers of the service)? (conceptually, not in your specific 
implementation). If yes, what would you do / change to prevent such abuse? 
 
 
If there are any questions / points to clarify please don’t hesitate to ask the person who 
gave you this task (or any other member of the team). 
 
 
