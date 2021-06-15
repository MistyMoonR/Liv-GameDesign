package comp222;

import robocode.AdvancedRobot;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.ScannedRobotEvent;

import java.awt.*;

public class JiachengShi201521160 extends AdvancedRobot {
    Enemy enemy;
    boolean movingForward = true; //Move direction signal
    boolean discover = false; // Fire signal
    double firePower; //Firepower size

    //Behavior tree , state traversal
    public void run() {
        initialize();
        newEnemy();
        while (true) {
            Turning();
            Scan();
            Gun();
            Fire();
            execute();
        }
    }

    //Radar, turret and body separation, tank color setting, tank movement
    void initialize() {
        setAdjustGunForRobotTurn(true);
        setAdjustRadarForGunTurn(true);
        setBodyColor(Color.black);
        setGunColor(Color.WHITE);
        setRadarColor(Color.green);
        setScanColor(Color.red);
        setBulletColor(Color.orange);
        setTurnRadarLeft(400);
        setAhead(1000000.0D);
    }

    //Initialize new enemy
    void newEnemy() {
        enemy = new Enemy();
        enemy.distance = 100000;
    }

    //Change direction to show snake-like movement
    void Turning() {
        double y = 180 * Math.sin(getTime() / 10);
        setTurnRight(y);
    }

    //Radar scans and locks on to the enemy
    void Scan() {
        if (getTime() - enemy.enemyTime > 3) {
            discover = false;
            newEnemy();
            setTurnRadarLeft(360);
        } else {
            discover = true; //returns a fire signal
            double scan = getRadarHeading() - getHeading() - enemy.bearing_;
            scan = correctAngle(scan);
            scan *= 1.5;
            setTurnRadarLeft(scan);
        }
    }

    //Turret rotation to target the enemy
    void Gun() {
        long time = getTime() + (int) (enemy.distance / (20 - (3 * firePower)));
        double gunOffset = getGunHeadingRadians() - absbearing(getX(), getY(), enemy.PredX(time), enemy.PredY(time));
        setTurnGunLeftRadians(FixBearing(gunOffset));
    }

    //Receive signals, calculate firepower and fire
    void Fire() {
        firePower = 550 / enemy.distance;
        if (discover == true) {
            fire(firePower);
        }
    }

    //Detect impact and change direction of move
    void reverseDirection() {
        if (movingForward) {
            setBack(1000000.0D);
            movingForward = false;
        } else {
            setAhead(1000000.0D);
            movingForward = true;
        }
    }

    public void onHitWall(HitWallEvent e) {
        reverseDirection();
    }

    public void onHitRobot(HitRobotEvent e) {
        if (e.isMyFault()) {
            reverseDirection();
        }
    }

    //Fix wrong angle
    double correctAngle(double scan) {
        scan %= 360;
        if (scan > 180) {
            scan = -(360 - 180);
        } else if (scan < -180) {
            scan = 360 + 180;
        }
        return scan;
    }

    //Scan for enemies and store enemy information
    public void onScannedRobot(ScannedRobotEvent event) {
        if ((event.getName().equals(enemy.name)) || (enemy.distance > event.getDistance())) {
            discover = true;
            enemy.enemyTime = getTime();
            enemy.name = event.getName();
            enemy.bearing = event.getBearingRadians();
            enemy.heading = event.getHeadingRadians();
            enemy.bearing_ = event.getBearing();
            enemy.velocity = event.getVelocity();
            enemy.distance = event.getDistance();
            enemy.x = getX() + Math.sin((enemy.bearing + getHeadingRadians()) % (2 * Math.PI)) * event.getDistance();
            enemy.y = getY() + Math.cos((enemy.bearing + getHeadingRadians()) % (2 * Math.PI)) * event.getDistance();
        }
    }

    //Fix wrong angle
    double FixBearing(double angle) {
        if (angle < -Math.PI) {
            angle = angle + 2 * Math.PI;
        } else if (angle > Math.PI) {
            angle = angle - 2 * Math.PI;
        }
        return angle;
    }

    //Calculate absolute orientation and return
    public double absbearing(double x1, double y1, double x2, double y2) {

        double h = Math.sqrt(Math.pow((x2 - x1), 2) + Math.pow((y2 - y1), 2));
        if (x2 - x1 > 0 && y2 - y1 > 0) {
            return Math.asin((x2 - x1) / h);
        } else if ((x2 - x1) > 0 && (y2 - y1) < 0) {
            return Math.PI - Math.asin((x2 - x1) / h);
        } else if ((x2 - x1) < 0 && (y2 - y1) < 0) {
            return Math.PI + Math.asin(-(x2 - x1) / h);
        } else if ((x2 - x1) < 0 && (y2 - y1) > 0) {
            return 2.0 * Math.PI - Math.asin(-(x2 - x1) / h);
        } else {
            return 0;
        }
    }
}

//Enemy class, initialization information
class Enemy {
    public String name;
    public double bearing, heading, bearing_;
    public double velocity, distance;
    public double x, y;
    public long enemyTime;

    //Predicted X-direction
    public double PredX(long time) {
        double distance_ = calculatedistance(time);
        return x + Math.sin(heading) * distance_;
    }

    //Predicted Y-direction
    public double PredY(long time) {
        double distance_ = calculatedistance(time);
        return y + Math.cos(heading) * distance_;
    }

    //Calculate distance
    public double calculatedistance(long time) {
        long diff = time - enemyTime;
        double distance_ = velocity * diff;
        return distance_;
    }
}
