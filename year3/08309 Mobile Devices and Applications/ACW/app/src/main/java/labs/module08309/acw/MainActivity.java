package labs.module08309.acw;


import android.app.Activity;
import android.content.Context;
import android.content.pm.ActivityInfo;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.os.AsyncTask;
import android.os.CountDownTimer;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Adapter;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.SimpleCursorAdapter;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLDecoder;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;
import java.util.logging.Handler;
import java.util.logging.LogRecord;

public class MainActivity extends AppCompatActivity implements SetupFragment.ListSelectionListener {
    private int puzzleListPos;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    // this method is used when an item from the list view is selected in the setup fragment to start to puzzle loading process
    @Override
    public void onListSelection(String puzzle, int pos) {
        puzzleListPos = pos;
        PuzzleFragment fragment = (PuzzleFragment) getFragmentManager().findFragmentById(R.id.puzzle_frag);
        fragment.showPuzzle(puzzle + ".json");
    }

    // this method is called in the puzzle fragment to update the puzzle list in the setup fragment
    public void UpdatePuzzleList() {
        SetupFragment fragment = (SetupFragment) getFragmentManager().findFragmentById(R.id.setup_frag);
        fragment.UpdateList(puzzleListPos);
    }

    public void buttonOnClick(View view) {
        final ImageButton button = (ImageButton) view.findViewById(R.id.imageButton);
        button.setBackgroundColor(Color.RED);
        button.postDelayed(new Runnable() {
            @Override
            public void run() {
                // Do something after 5s = 5000ms
                button.setBackgroundColor(Color.BLACK);
            }
        }, 1000);
    }

    // this method locks the orientation
    public void LockOri()
    {
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
    }

    // this method unlocks the orientation
    public void UnlockOri()
    {
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED);
    }
}


